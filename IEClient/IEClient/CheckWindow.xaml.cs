﻿using Brilliantech.Framwork.Utils.LogUtil;
using ClearInsight;
using ClearInsight.Model;
using IEClient.Config;
using IEClient.Properties;
using IEClientLib;
using IEClientLib.Enums;
using MahApps.Metro.Controls;
using System;
using System.Collections;
using System.Collections.Generic;
using System.ComponentModel;
using System.Linq;
using System.Text;
using System.Threading;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Data;
using System.Windows.Documents;
using System.Windows.Input;
using System.Windows.Media;
using System.Windows.Media.Imaging;
using System.Windows.Navigation;
using System.Windows.Shapes;
using System.Windows.Threading;

namespace IEClient
{
    /// <summary>
    /// CheckPage.xaml 的交互逻辑
    /// </summary>
    public partial class CheckWindow : MetroWindow
    {
        IEHost<Node> ieHost;
        List<IESlave<Node>> ieSlaves;

        Queue slaveDataSyncQueue;
        Queue slaveDataQueue;
        Thread slaveDataHandlerThread;
        ManualResetEvent slaveDataEvent;

        public CheckWindow()
        {
            InitializeComponent();
        }
        private void MetroWindow_Loaded(object sender, RoutedEventArgs e)
        {
            LoadData();
        }
        /// <summary>
        /// 加载数据
        /// </summary>
        public void LoadData()
        {
            ClearInsightAPI ci = new ClearInsightAPI(BaseConfig.Server, UserSession.GetInstance().CurrentUser.token);
            List<Node> nodes = ci.GetWorkUnitNodes(UserSession.GetInstance().CurrentProject.id);
            ieSlaves = new List<IESlave<Node>>();
            foreach (Node node in nodes)
            {
                IESlave<Node> slave = new IESlave<Node>()
                {
                    Id = node.id,
                    ExtItem = node,
                    Name = node.name,
                    Code = node.devise_code
                };
                slave.TimeTicked += new IESlave<Node>.TimeTickedEventHandler(Slave_TimeTicked);
                ieSlaves.Add(slave);
                }
            this.UniformGrid.DataContext = ieSlaves;
            ieHost = new IEHost<Node>(BaseConfig.Com,BaseConfig.BaundRate,BaseConfig.Parity,BaseConfig.TimeOut);
            //ieHost.Slaves = ieSlaves;
        }


        private void to_Item_Click(object sender, RoutedEventArgs e)
        {
            ItemsWindow win = new ItemsWindow();
            win.Show();
            this.Close();
        }
        private void detail_Click(object sender, RoutedEventArgs e)
        {
            ItemsWindow page = new ItemsWindow();
            page.Content = new DetailPage();
            page.WindowState = System.Windows.WindowState.Normal;
            page.Topmost = true;
            page.Show();
        }
        //范围设置
        private void range_set_Click(object sender, RoutedEventArgs e)
        {
            SettingBoxWindow win = new SettingBoxWindow();
            win.ShowDialog(); 
        }

        private void finish_Click(object sender, RoutedEventArgs e)
        {
            if (MessageBox.Show("确定结束测试？", "确定结束测试？", MessageBoxButton.YesNo, MessageBoxImage.Question) == MessageBoxResult.Yes)
            {
                try
                {
                    if (ieHost != null)
                    {
                        ieHost.StopTest();
                        begin.IsEnabled = true;
                        finish.IsEnabled = false;
                        itemWindow.IsEnabled = true;
                    }
                }
                catch (Exception ex)
                {
                    LogUtil.Logger.Error(ex.Message);
                    LogUtil.Logger.Error(ex.Source);
                    begin.IsEnabled = false;
                    finish.IsEnabled = true;
                    itemWindow.IsEnabled = false;
                    MessageBox.Show(ex.Message);
                }
            }
        }

        /// <summary>
        /// 全选
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void allCheckBox_Click(object sender, RoutedEventArgs e)
        {
            foreach (IESlave<Node> slave in ieSlaves)
            {
                slave.Selected = true;               
            }

        }
        private void allCheckBox_Uncheck(object sender, RoutedEventArgs e)
        {
            foreach (IESlave<Node> slave in ieSlaves)
            {
                slave.Selected = false;
            }
        }

        /// <summary>
        /// 绑定设备
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void binding_Click(object sender, MouseButtonEventArgs e)
        {
            Point mouse_position = Mouse.GetPosition(e.Source as FrameworkElement);
            Point positionToscreen = (e.Source as FrameworkElement).PointToScreen(mouse_position);

            IESlave<Node> slave = this.UniformGrid.SelectedItem as IESlave<Node>;

            BindingWindow win = new BindingWindow() { slave = slave, PositionX = positionToscreen.X, PositionY=positionToscreen.Y};
            win.ShowDialog();
        }

        /// <summary>
        /// 开始测试
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void begin_Click(object sender, RoutedEventArgs e)
        {
            try
            {
                begin.IsEnabled = false;
                finish.IsEnabled = true;
 

                ieHost.Slaves = GetSelectedSlaves();
                /// 开始测试
              //  this.Dispatcher.Invoke(DispatcherPriority.Normal, (System.Windows.Forms.MethodInvoker)delegate ()
               // {
                    ieHost.StartTest();
                    ieHost.PollData();
              //  });

                 
            }
            catch (Exception ex)
            {
                LogUtil.Logger.Error(ex.Message);
                LogUtil.Logger.Error(ex.Source);
                begin.IsEnabled = true;
                finish.IsEnabled = false;
                itemWindow.IsEnabled = true;
                MessageBox.Show(ex.Message);
            }
        }

        private List<IESlave<Node>> GetSelectedSlaves() {
            return this.ieSlaves.Where(s => s.Selected == true).ToList();
        }


        /// <summary>
        /// 从机开始计时了
        /// </summary>
        /// <param name="slave"></param>
        /// <param name="data"></param>
        private void Slave_TimeTicked(IESlave<Node> slave, IEData data)
        {
            if (slaveDataSyncQueue == null)
            {
                slaveDataSyncQueue = new Queue();
                slaveDataHandlerThread = new Thread(this.SlaveDataThreadHandler);
                slaveDataQueue = Queue.Synchronized(slaveDataSyncQueue);

                slaveDataEvent=new ManualResetEvent(false);
                slaveDataHandlerThread.Start();
            }
            Dictionary<string, object> dic = new Dictionary<string, object>();

            dic.Add("slave", slave);
            dic.Add("data", data);

            slaveDataQueue.Enqueue(dic);
            slaveDataEvent.Set();
        }

        private void SlaveDataThreadHandler()
        {
            while (true)
            {
                while (slaveDataQueue.Count > 0)
                {
                    SlaveDataHandler((Dictionary<string,object>)slaveDataQueue.Dequeue());
                }
                slaveDataEvent.WaitOne();
                slaveDataEvent.Reset();
            }
        }

        private void SlaveDataHandler(Dictionary<string, object> dic) {
            IESlave<Node> slave = dic["slave"] as IESlave<Node>;
            IEData data = dic["data"] as IEData;

            ClearInsightAPI api = new ClearInsightAPI(BaseConfig.Server, UserSession.GetInstance().CurrentUser.token);

            KpiEntry entry = new KpiEntry()
            {
                kpi_code = BaseConfig.CycleTimeKpiCode,
                entry_at = DateTime.Now,
                project_item_id = slave.ExtItem.project_item_id,
                tenant_id = slave.ExtItem.tenant_id,
                node_id = slave.ExtItem.id,
                node_code = slave.ExtItem.code,
                node_uuid = slave.ExtItem.uuid,
                value = data.Time/10
            };

            KpiEntry back = api.UploadKpiEntry(entry);
        }

        private void MetroWindow_Closing(object sender, CancelEventArgs e)
        {
            if (slaveDataHandlerThread != null)
            {
                int count = slaveDataQueue.Count;
                if (count>0) {
                    e.Cancel=true;
                    MessageBox.Show(string.Format("仍有{0}条数据在处理中，请稍候...", count),"数据处理提醒",MessageBoxButton.OK,MessageBoxImage.Warning);
                }
                else {
                    slaveDataHandlerThread.Abort();
                }
            }
        }

        private void settingWindow_Click(object sender, RoutedEventArgs e)
        {
        }
    }
}
