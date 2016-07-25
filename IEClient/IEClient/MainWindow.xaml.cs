using MahApps.Metro.Controls;
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
using System.Windows.Navigation;
using System.Windows.Shapes;
using ClearInsight;
using ClearInsight.Model;
using IEClient.Properties;
using IEClient.Config;

namespace IEClient
{
    /// <summary>
    /// MainWindow.xaml 的交互逻辑
    /// </summary>
    public partial class MainWindow : MetroWindow
    {
        public MainWindow()
        {
            InitializeComponent();
            double screenHeight = SystemParameters.FullPrimaryScreenHeight;
            double screenWidth = SystemParameters.FullPrimaryScreenWidth;
            this.Top = (screenHeight - this.Height) / 2;
            this.Left = (screenWidth - this.Width) / 2;
             
            // preset
            email.Text = "admin@ci.com";
            password.Password = "123456@";
        }
        

        private void login_Click(object sender, RoutedEventArgs e)
        {
            ClearInsightAPI ci = new ClearInsightAPI(BaseConfig.Server);
            Msg<User> msg = ci.UserLogin(email.Text.Trim(), password.Password.Trim());
            if (msg.result)
            {
                UserSession.GetInstance().CurrentUser = msg.data;
                ItemsWindow win = new ItemsWindow();
                win.Show();
                this.Close();
            }
            else
            {
                MessageBox.Show("登录失败，请核实邮件和密码");
            }
        }
        private void Window_MouseDown(object sender, MouseButtonEventArgs e)
        {
            if (e.LeftButton == MouseButtonState.Pressed)
            {
                DragMove();
            }
        }
        private void mouseenter(object enter, RoutedEventArgs e)
        {
            MessageBox.Show(email.Text);
        }
    }

  
}
