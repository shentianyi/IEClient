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

namespace IEClient
{
    /// <summary>
    /// SettingBoxWindow.xaml 的交互逻辑
    /// </summary>
    public partial class SettingBoxWindow : Window
    {
        public SettingBoxWindow()
        {
            InitializeComponent();
            //this.Topmost = true;
            double screenHeight = SystemParameters.FullPrimaryScreenHeight;
            double screenWidth = SystemParameters.FullPrimaryScreenWidth;
            this.Top = (screenHeight - this.Height) / 2;
            this.Left = (screenWidth - this.Width) / 2;
        }
        private void Window_MouseDown(object sender, MouseButtonEventArgs e)
        {
            if (e.LeftButton == MouseButtonState.Pressed)
            {
                DragMove();
            }
        }
        private void cancel_Setting_Click(object sender, RoutedEventArgs e)
        {
            this.Close();
        }
        private void save_Setting_Click(object sender, RoutedEventArgs e)
        {
            this.Close();
        }

    }
}
