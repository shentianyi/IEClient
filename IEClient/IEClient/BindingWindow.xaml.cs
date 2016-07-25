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
    /// BindingWindow.xaml 的交互逻辑
    /// </summary>
    public partial class BindingWindow : Window
    {
        public static string device;
        public BindingWindow()
        {
            InitializeComponent();
            double screenHeight = SystemParameters.FullPrimaryScreenHeight;
            double screenWidth = SystemParameters.FullPrimaryScreenWidth;
            this.Top = ItemBindingWindow.position_y;
            this.Left = ItemBindingWindow.position_x;
            if (screenWidth - ItemBindingWindow.position_x < this.Width)
                {
                    this.Left = (ItemBindingWindow.position_x - this.Width);
                }
            if (screenHeight - ItemBindingWindow.position_y < this.Height)
                {
                    this.Top = (ItemBindingWindow.position_y - this.Height);
                }


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

            device = deviceID.Text;
            this.Close();
        }
    }
}
