using ClearInsight.Model;
using IEClientLib;
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

        public List<IESlave<Node>> IESlaves { get; set; }

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
            try
            {
                float? max = null;
                float? min = null;

                if (!string.IsNullOrEmpty(maxNumber.Text))
                {
                    max = float.Parse(maxNumber.Text);
                }
                if (!string.IsNullOrEmpty(minNumber.Text))
                {
                    min = float.Parse(minNumber.Text);
                }
                foreach (IESlave<Node> node in IESlaves)
                {
                    node.MaxFilter = max;
                    node.MinFilter = min;
                }

                this.Close();

            }
            catch (Exception ex)
            {
                MessageBox.Show(ex.Message);
            }
        }

    }
}
