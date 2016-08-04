using MahApps.Metro.Controls;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Data;
using System.Windows.Documents;
using System.Windows.Input;
using System.Windows.Media;
using System.Windows.Media.Imaging;
using System.Windows.Shapes;
using System.Text.RegularExpressions;
using IEClient.Config;
using System.IO.Ports;

namespace IEClient
{
    /// <summary>
    /// SetterWindow.xaml 的交互逻辑
    /// </summary>
    public partial class SetterWindow : MetroWindow
    {
        public SetterWindow()
        {
            InitializeComponent();
        }

        private void save_Click(object sender, RoutedEventArgs e)
        {
            try {
                BaseConfig.Server = this.ServerText.Text;
                BaseConfig.Com = this.ComCB.SelectedItem.ToString();
                BaseConfig.BaundRate = int.Parse(this.BaudRateText.Text);

                MessageBox.Show("设置成功", "提示", MessageBoxButton.OK, MessageBoxImage.Asterisk);
                this.Close();
            }
            catch(Exception ex)
            {
                MessageBox.Show( string.Format("设置错误：{0}", ex.Message), "提示", MessageBoxButton.OK, MessageBoxImage.Error);
            }
        }

        private void Baud_PreviewTextInput(object sender, TextCompositionEventArgs e)
        {
            Regex re = new Regex("[^0-9]");
            e.Handled = re.IsMatch(e.Text);
        }

        private void MetroWindow_Loaded(object sender, RoutedEventArgs e)
        {
            this.ServerText.Text = BaseConfig.Server;
            //this.ComText.Text = BaseConfig.Com;
            this.BaudRateText.Text = BaseConfig.BaundRate.ToString();

            ComCB.ItemsSource= SerialPort.GetPortNames();

            for(int i = 0; i < ComCB.Items.Count; i++)
            {

                if (BaseConfig.Com.Equals(ComCB.Items[i].ToString()))
                {
                    ComCB.SelectedIndex = i;
                    break;
                }
            }

            if (ComCB.SelectedIndex < 0)
            {
                ComCB.SelectedIndex = 0;
            }
        }
    }
}
