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
using ClearInsight.Model;
using ClearInsight;
using IEClient.Properties;
using IEClient.Config;
using IEClientLib;

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

        private void binding_Click(object sender, MouseButtonEventArgs e)
        {

            Point mouse_position = Mouse.GetPosition(e.Source as FrameworkElement);
            Point positionToscreen = (e.Source as FrameworkElement).PointToScreen(mouse_position);
            position_x = positionToscreen.X;
            position_y = positionToscreen.Y;
            IESlave slave = this.UniformGrid.SelectedItem as IESlave;

            BindingWindow win = new BindingWindow() { slave= slave };
            win.ShowDialog();
        }

        private void LoadData()
        {
            ClearInsightAPI ci = new ClearInsightAPI(BaseConfig.Server, UserSession.GetInstance().CurrentUser.token);
            List<Node> nodes = ci.GetWorkUnitNodes(UserSession.GetInstance().CurrentProject.id);
            this.UniformGrid.DataContext =nodes;
        } 
        
        private void save_test(object sender, RoutedEventArgs e)
        {
        }
    }
}
