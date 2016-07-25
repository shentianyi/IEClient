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

        public CheckWindow()
        {
            InitializeComponent();

            LoadData();
        }
        private void LoadData()
        {
            List<Student> students = new List<Student>
            {
                new Student { Device="设备1", Station=80, Gender=true, Electricity=75},
                new Student { Device="设备2", Station=80, Gender=false, Electricity=75},
                new Student { Device="设备3", Station=80, Gender=true, Electricity=75},
                new Student { Device="设备4", Station=80, Gender=true, Electricity=80},
                new Student { Device="设备5", Station=80, Gender=false, Electricity=75},
                new Student { Device="设备6", Station=80, Gender=true, Electricity=75},
                new Student { Device="设备7", Station=80, Gender=true, Electricity=100},
                new Student { Device="设备8", Station=80, Gender=true, Electricity=75},
                new Student { Device="设备9", Station=80, Gender=true, Electricity=23},
                new Student { Device="设备10", Station=80, Gender=true, Electricity=75},
                new Student { Device="设备11", Station=80, Gender=true, Electricity=100},
                new Student { Device="设备12", Station=80, Gender=true, Electricity=75},
                new Student { Device="设备13", Station=80, Gender=true, Electricity=68},
                new Student { Device="设备14", Station=80, Gender=true, Electricity=100},
                new Student { Device="设备15", Station=80, Gender=true, Electricity=74},
                new Student { Device="设备16", Station=80, Gender=true, Electricity=100},
                new Student { Device="设备17", Station=80, Gender=true, Electricity=75},
                new Student { Device="设备18", Station=80, Gender=true, Electricity=100},
            };
            this.UniformGrid.DataContext = students;
        }
        public class Student
        {
            public string Device { get; set; }
            public int Station { get; set; }
            public bool Gender { get; set; }
            public int Electricity { get; set; }
        }

        private void to_Item_Click(object sender, RoutedEventArgs e)
        {
            //page 转 window
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
            //win.Show();
            win.ShowDialog();
            
            //Window page = (Window)this.Parent;
           // page.Close();
        }
        private void finish_Click(object sender, RoutedEventArgs e)
        {
            MessageBox.Show("确定退出？");
        }
        private void checkBox_Click(object sender, RoutedEventArgs e)
        {
            MessageBox.Show("queding");
        }

        private void binding_Click(object sender, MouseButtonEventArgs e)
        {

        }
    }
}
