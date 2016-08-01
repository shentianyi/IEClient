using Brilliantech.Framwork.Utils.LogUtil;
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
            if (finish.IsEnabled == false)
            {
                ItemsWindow win = new ItemsWindow();
                win.Show();
                this.Close();
            }
            else {
                MessageBox.Show("请先结束测试");
            }
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
            SettingBoxWindow win = new SettingBoxWindow() { IESlaves=GetSelectedSlaves()};
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
<<<<<<< HEAD
=======
                        
>>>>>>> b6277c7c0186d8180c07b13b1c89a9928724642b
                        begin.IsEnabled = true;
                        finish.IsEnabled = false;
                    }
                }
                catch (Exception ex)
                {
                    LogUtil.Logger.Error(ex.Message);
                    LogUtil.Logger.Error(ex.Source);
                    begin.IsEnabled = false;
                    finish.IsEnabled = true;
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
<<<<<<< HEAD
                finish.IsEnabled = true;
 

                ieHost.Slaves = GetSelectedSlaves();
                /// 开始测试
              //  this.Dispatcher.Invoke(DispatcherPriority.Normal, (System.Windows.Forms.MethodInvoker)delegate ()
               // {
                    ieHost.StartTest();
                    ieHost.PollData();
              //  });

                 
=======
                finish.IsEnabled = true;   
                foreach (IESlave<Node> slave in ieSlaves)
                {
                    if (slave.Selected == true) {
                        /// 开始测试
                        this.Dispatcher.Invoke(DispatcherPriority.Normal, (System.Windows.Forms.MethodInvoker)delegate ()
                        {
                            ieHost.StartTest();
                            ieHost.PollData();
                        });
                    };
                }
        
>>>>>>> b6277c7c0186d8180c07b13b1c89a9928724642b
            }
            catch (Exception ex)
            {
                LogUtil.Logger.Error(ex.Message);
                LogUtil.Logger.Error(ex.Source);
                begin.IsEnabled = true;
                finish.IsEnabled = false;
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
            float value =  data.Time / 10;
            bool up = false;

            LogUtil.Logger.Info(string.Format("slave: {0}, value is: {1}, maxfilter: {2} , minfilter: {3}",slave.Code,value,slave.MaxFilter,slave.MinFilter));
            if (slave.MinFilter.HasValue) {
                if (value >= slave.MinFilter.Value) {
                    up = true;
                }
            }
            else
            {
                up = true;
            }

            if (slave.MaxFilter.HasValue)
            {
                if (up)
                {
                    if (value <= slave.MaxFilter.Value)
                    {
                        up = true;
                    }
                    else { up = false; }
                }
            }

            if (up)
            {

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
                    value = data.Time / 10
                };

                KpiEntry back = api.UploadKpiEntry(entry);
                data.Stored = true;
            }
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
        protected override void OnClosing(System.ComponentModel.CancelEventArgs e)
        {
            if (begin.IsEnabled == false)
            {
                e.Cancel = true;
            }
            else {
                e.Cancel = false;
            }
            
        }

        private void settingWindow_Click(object sender, RoutedEventArgs e)
        {
        }
    }
}
