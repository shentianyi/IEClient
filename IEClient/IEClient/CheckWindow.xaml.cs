using Brilliantech.Framwork.Utils.LogUtil;
using ClearInsight;
using ClearInsight.Model;
using IEClient.Config;
using IEClient.Properties;
using IEClientLib;
using MahApps.Metro.Controls;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Linq;
using System.Text;
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

namespace IEClient
{
    /// <summary>
    /// CheckPage.xaml 的交互逻辑
    /// </summary>
    public partial class CheckWindow : MetroWindow
    {
        IEHost ieHost;
        List<IESlave> ieSlaves;
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
        private void LoadData()
        {
            ClearInsightAPI ci = new ClearInsightAPI(BaseConfig.Server, UserSession.GetInstance().CurrentUser.token);
            List<Node> nodes = ci.GetWorkUnitNodes(UserSession.GetInstance().CurrentProject.id);
            ieSlaves = new List<IESlave>();
            foreach (Node node in nodes)
            {
                ieSlaves.Add(
                    new IESlave()
                    {
                        Id = node.id,
                        ExtId = node.id,
                        ExtCode = node.code,
                        Name = node.name,
                        Code = node.devise_code
                    });
            }
            this.UniformGrid.DataContext = ieSlaves;
            ieHost = new IEHost(BaseConfig.Com);
            ieHost.Slaves = ieSlaves;
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

            IESlave slave = this.UniformGrid.SelectedItem as IESlave;

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
                /// 开始测试
                ieHost.StartTest();
                ieHost.PollData();
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
    }
}
