using System;
using System.Collections.Generic;
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
using System.Windows.Shapes;
using ClearInsight;
using ClearInsight.Model;
using IEClient.Properties;
using IEClient.Config;
using IEClientLib;

namespace IEClient
{
    /// <summary>
    /// BindingWindow.xaml 的交互逻辑
    /// </summary>
    public partial class BindingWindow : Window
    {
        public IESlave<Node> slave { get; set; }
        ClearInsightAPI ci;

        public double PositionX { get; set; }
        public double PositionY { get; set; }

        public BindingWindow()
        {
            InitializeComponent();
           
            
        }

        private void Window_MouseDown(object sender, MouseButtonEventArgs e)
        {
            if (e.LeftButton == MouseButtonState.Pressed)
            {
                DragMove();
            }
        }
        private void cancel_Click(object sender, RoutedEventArgs e)
        {
            this.Close();
        }
        private void save_Click(object sender, RoutedEventArgs e)
        {
            bindDevice();
        }

        private void deviceID_KeyUp(object sender, KeyEventArgs e)
        {
            if (e.Key == Key.Enter)
            {
                bindDevice();
            }
        }

        private void bindDevice()
        {
            ClearInsightAPI ci = new ClearInsightAPI(BaseConfig.Server, UserSession.GetInstance().CurrentUser.token);

            if (string.IsNullOrWhiteSpace(deviceID.Text.Trim()))
            {
                MessageBox.Show("请输入设备编码");
            }
            else
            {
                this.slave.Code = deviceID.Text.Trim();
                ci.BindNodeDevise(this.slave.Id, this.slave.Code);
                this.Close();
            }
        }

        private void Window_Loaded(object sender, RoutedEventArgs e)
        {
            this.deviceID.Text = this.slave.Code;
            this.deviceID.Focus();
            this.deviceID.SelectAll();

            double screenHeight = SystemParameters.FullPrimaryScreenHeight;
            double screenWidth = SystemParameters.FullPrimaryScreenWidth;
            this.Top = this.PositionY;
            this.Left = this.PositionX;
            if (screenWidth - this.PositionX < this.Width)
            {
                this.Left = (this.PositionX - this.Width);
            }
            if (screenHeight - this.PositionY < this.Height)
            {
                this.Top = (this.PositionY - this.Height);
            }
        }
    }
}
