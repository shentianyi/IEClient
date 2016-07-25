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
using System.Windows.Shapes;

namespace IEClient
{
    /// <summary>
    /// ItemBindingWindow.xaml 的交互逻辑
    /// </summary>
    public partial class ItemBindingWindow : MetroWindow
    {
        public static double position_x;
        public static double position_y;
        public ItemBindingWindow()
        {
            InitializeComponent();
            LoadData();
            this.WindowState = System.Windows.WindowState.Maximized;
        }
        
            private void binding_Click(object sender, SelectionChangedEventArgs e)
        {
 
            Point mouse_position = Mouse.GetPosition(e.Source as FrameworkElement);
            Point positionToscreen = (e.Source as FrameworkElement).PointToScreen(mouse_position);
            position_x = positionToscreen.X;
            position_y = positionToscreen.Y;
           //MessageBox.Show(string.Format("GetCursorPos {0},{1}", position_x, position_y));
           //MessageBox.Show(string.Format("GetCursorPos {0},{1}  GetPosition {2},{3}\r\n {4},{5}", p.X, p.Y, pp.X, pp.Y, ppp.X, ppp.Y));
            BindingWindow win = new BindingWindow();
            win.ShowDialog();
        }

        private void LoadData()
        {
            List<Student> students = new List<Student>
            {
                new Student { Station=BindingWindow.device, Device="设备一"},
                new Student { Station="工位二", Device="设备二"},
                new Student { Station="工位三", Device="设备三"},
                new Student { Station="工位四", Device="设备四"},
                new Student { Station="工位五", Device="设备五"},
                new Student { Station="工位六", Device="设备六"},
                new Student { Station="工位一", Device="设备一"},
                new Student { Station="工位二", Device="设备二"},
                new Student { Station="工位三", Device="设备三"},
                new Student { Station="工位四", Device="设备四"},
                new Student { Station="工位五", Device="设备五"},
                new Student { Station="工位六", Device="设备六"},
                new Student { Station="工位一", Device="设备一"},
                new Student { Station="工位二", Device="设备二"},
                new Student { Station="工位三", Device="设备三"},
                new Student { Station="工位四", Device="设备四"},
                new Student { Station="工位五", Device="设备五"},
                new Student { Station="工位六", Device="设备六"},
                new Student { Station="工位一", Device="设备一"},
                new Student { Station="工位二", Device="设备二"},
                new Student { Station="工位三", Device="设备三"},
                new Student { Station="工位四", Device="设备四"},
                new Student { Station="工位五", Device="设备五"},
                new Student { Station="工位六", Device="设备六"},
                 new Student { Station="工位一", Device="设备一"},
                new Student { Station="工位二", Device="设备二"},
                new Student { Station="工位三", Device="设备三"},
                new Student { Station="工位四", Device="设备四"},
                new Student { Station="工位五", Device="设备五"},
                new Student { Station="工位六", Device="设备六"},
                new Student { Station="工位一", Device="设备一"},
                new Student { Station="工位二", Device="设备二"},
                new Student { Station="工位三", Device="设备三"},
                new Student { Station="工位四", Device="设备四"},
                new Student { Station="工位五", Device="设备五"},
                new Student { Station="工位六", Device="设备六"},
                new Student { Station="工位一", Device="设备一"},
                new Student { Station="工位二", Device="设备二"},
                new Student { Station="工位三", Device="设备三"},
                new Student { Station="工位四", Device="设备四"},
                new Student { Station="工位五", Device="设备五"},
                new Student { Station="工位六", Device="设备六"},
                new Student { Station="工位一", Device="设备一"},
                new Student { Station="工位二", Device="设备二"},
                new Student { Station="工位三", Device="设备三"},
                new Student { Station="工位四", Device="设备四"},
                new Student { Station="工位五", Device="设备五"},
                new Student { Station="工位六", Device="设备六"},
                 new Student { Station="工位一", Device="设备一"},
                new Student { Station="工位二", Device="设备二"},
                new Student { Station="工位三", Device="设备三"},
                new Student { Station="工位四", Device="设备四"},
                new Student { Station="工位五", Device="设备五"},
                new Student { Station="工位六", Device="设备六"},
                new Student { Station="工位一", Device="设备一"},
                new Student { Station="工位二", Device="设备二"},
                new Student { Station="工位三", Device="设备三"},
                new Student { Station="工位四", Device="设备四"},
                new Student { Station="工位五", Device="设备五"},
                new Student { Station="工位六", Device="设备六"},
                new Student { Station="工位一", Device="设备一"},
                new Student { Station="工位二", Device="设备二"},
                new Student { Station="工位三", Device="设备三"},
                new Student { Station="工位四", Device="设备四"},
                new Student { Station="工位五", Device="设备五"},
                new Student { Station="工位六", Device="设备六"},
                new Student { Station="工位一", Device="设备一"},
                new Student { Station="工位二", Device="设备二"},
                new Student { Station="工位三", Device="设备三"},
                new Student { Station="工位四", Device="设备四"},
                new Student { Station="工位五", Device="设备五"},
                new Student { Station="工位六", Device="设备六"},
                 new Student { Station="工位一", Device="设备一"},
                new Student { Station="工位二", Device="设备二"},
                new Student { Station="工位三", Device="设备三"},
                new Student { Station="工位四", Device="设备四"},
                new Student { Station="工位五", Device="设备五"},
                new Student { Station="工位六", Device="设备六"},
                new Student { Station="工位一", Device="设备一"},
                new Student { Station="工位二", Device="设备二"},
                new Student { Station="工位三", Device="设备三"},
                new Student { Station="工位四", Device="设备四"},
                new Student { Station="工位五", Device="设备五"},
                new Student { Station="工位六", Device="设备六"},
                new Student { Station="工位一", Device="设备一"},
                new Student { Station="工位二", Device="设备二"},
                new Student { Station="工位三", Device="设备三"},
                new Student { Station="工位四", Device="设备四"},
                new Student { Station="工位五", Device="设备五"},
                new Student { Station="工位六", Device="设备六"},
                new Student { Station="工位一", Device="设备一"},
                new Student { Station="工位二", Device="设备二"},
                new Student { Station="工位三", Device="设备三"},
                new Student { Station="工位四", Device="设备四"},
                new Student { Station="工位五", Device="设备五"},
                new Student { Station="工位六", Device="设备六"},
                    new Student { Station="工位三", Device="设备三"},
                new Student { Station="工位四", Device="设备四"},
                new Student { Station="工位五", Device="设备五"},
                new Student { Station="工位六", Device="设备六"},
                new Student { Station="工位一", Device="设备一"},
                new Student { Station="工位二", Device="设备二"},
                new Student { Station="工位三", Device="设备三"},
                new Student { Station="工位四", Device="设备四"},
                new Student { Station="工位五", Device="设备五"},
                new Student { Station="工位六", Device="设备六"},
                new Student { Station="工位一", Device="设备一"},
                new Student { Station="工位二", Device="设备二"},
                new Student { Station="工位三", Device="设备三"},
                new Student { Station="工位四", Device="设备四"},
                new Student { Station="工位五", Device="设备五"},
                new Student { Station="工位六", Device="设备六"},
                 new Student { Station="工位一", Device="设备一"},
                new Student { Station="工位二", Device="设备二"},
                new Student { Station="工位三", Device="设备三"},
                new Student { Station="工位四", Device="设备四"},
                new Student { Station="工位五", Device="设备五"},
                new Student { Station="工位六", Device="设备六"},
                new Student { Station="工位一", Device="设备一"},
                new Student { Station="工位二", Device="设备二"},
                new Student { Station="工位三", Device="设备三"},
                new Student { Station="工位四", Device="设备四"},
                new Student { Station="工位五", Device="设备五"},
                new Student { Station="工位六", Device="设备六"},
                new Student { Station="工位一", Device="设备一"},
                new Student { Station="工位二", Device="设备二"},
                new Student { Station="工位三", Device="设备三"},
                new Student { Station="工位四", Device="设备四"},
                new Student { Station="工位五", Device="设备五"},
                new Student { Station="工位六", Device="设备六"},
                new Student { Station="工位一", Device="设备一"},
                new Student { Station="工位二", Device="设备二"},
                new Student { Station="工位三", Device="设备三"},
                new Student { Station="工位四", Device="设备四"},
                new Student { Station="工位五", Device="设备五"},
                new Student { Station="工位六", Device="设备六"},
            };
            this.UniformGrid.DataContext = students;
        }
        public class Student
        {
            public string Station { get; set; }
            public string Device { get; set; }

        }
        
        private void save_test(object sender, RoutedEventArgs e)
        {
        }
    }
}
