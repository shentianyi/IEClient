using MahApps.Metro.Controls;
using MahApps.Metro.Native;
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

namespace IEClient
{
    /// <summary>
    /// ItemsWindow.xaml 的交互逻辑
    /// </summary>
    public partial class ItemsWindow : MetroWindow
    {
        public ItemsWindow()
        {
            InitializeComponent();
            this.WindowState = System.Windows.WindowState.Maximized;
            this.WindowStyle = System.Windows.WindowStyle.None;
            LoadData();
        }

        private void LoadData()
        {
            List<Student> students = new List<Student>
            {
                new Student { Item="项目一", Note="一些说明"},
                new Student { Item="项目二", Note="一些说明"},
                new Student { Item="项目三", Note="一些说明"},
                new Student { Item="项目四", Note="一些说明"},
                new Student { Item="项目五", Note="一些说明"},
                new Student { Item="项目六", Note="一些说明"},
            };
            this.UniformGrid.DataContext = students;
        }
        public class Student
        {
            public string Item { get; set; }
            public string Note { get; set; }

        }
        private void to_Check_Cleck(object sender, RoutedEventArgs e)
        {
            // MessageBox.Show("hh");
            // Application.Current.Shutdown();
           // CheckWindow win = new CheckWindow();
            //CheckPage page = new CheckPage();
            //win.Content = new ResPage();
            ItemsWindow win = new ItemsWindow();
            win.Content = new CheckPage();
            win.Title = "设备检测";                      
            win.Show();
            this.Close();
        }
        private void to_ItemBinding_Click(object sender, RoutedEventArgs e)
        {
            ItemBindingWindow win = new ItemBindingWindow();
            win.ShowDialog();

        }

    }
}
