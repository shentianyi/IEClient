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
using IEClientLib;
using ClearInsight.Model;

namespace IEClient
{
    /// <summary>
    /// SetterWindow.xaml 的交互逻辑
    /// </summary>
    public partial class StatusWarnSettingWindow : MetroWindow
    {
        public List<IESlave<Node>> slaves = new List<IESlave<Node>>();
        public StatusWarnSettingWindow()
        {
            InitializeComponent();
        }

        private void save_Click(object sender, RoutedEventArgs e)
        {
            try
            {
                BaseConfig.OutClockingMax = int.Parse(this.OutClockingMaxText.Text);
                BaseConfig.OnClockingMax = int.Parse(this.OnClockingMaxText.Text);
                foreach(var slave in slaves)
                {
                    slave.OutClockingMax = BaseConfig.OutClockingMax;
                    slave.OnClockingMax = BaseConfig.OnClockingMax;
                }
                MessageBox.Show("设置成功", "提示", MessageBoxButton.OK, MessageBoxImage.Asterisk);
                this.Close();
            }
            catch (Exception ex)
            {
                MessageBox.Show(string.Format("设置错误：{0}", ex.Message), "提示", MessageBoxButton.OK, MessageBoxImage.Error);
            }
        }

      

        private void MetroWindow_Loaded(object sender, RoutedEventArgs e)
        {
            this.OutClockingMaxText.Text = BaseConfig.OutClockingMax.ToString();
            this.OnClockingMaxText.Text = BaseConfig.OnClockingMax.ToString();
        }
    }
}
