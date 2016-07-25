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

namespace IEClient
{
    /// <summary>
    /// CheckPage.xaml 的交互逻辑
    /// </summary>
    public partial class CheckPage : Page
    {
        public static double position_x;
        public static double position_y;

        private bool underTesting = false;

        public CheckPage()
        {
            InitializeComponent();
            LoadData();
        }
        private void LoadData()
        {
            ClearInsightAPI ci = new ClearInsightAPI(Settings.Default.BaseUrl, UserSession.GetInstance().CurrentUser.token);
            List<Node> nodes = ci.GetWorkUnitNodes(UserSession.GetInstance().CurrentProject.id);
            this.UniformGrid.DataContext = nodes;
        }
       

        private void to_Item_Click(object sender, RoutedEventArgs e)
        {
            //page 转 window
            ItemsWindow win = new ItemsWindow();
            win.Title = "项目管理";
            win.Show();
            Window page = (Window)this.Parent;
            page.Close();
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
      
            win.ShowDialog();
        }
        private void finish_Click(object sender, RoutedEventArgs e)
        {
            MessageBox.Show("确定退出？");
        }


        private void binding_Click(object sender, MouseButtonEventArgs e)
        {

            Point mouse_position = Mouse.GetPosition(e.Source as FrameworkElement);
            Point positionToscreen = (e.Source as FrameworkElement).PointToScreen(mouse_position);
            position_x = positionToscreen.X;
            position_y = positionToscreen.Y;
            Node node = this.UniformGrid.SelectedItem as Node;

            BindingWindow win = new BindingWindow() { node = node,PositionX= position_x,PositionY=position_y };
            win.ShowDialog();
        }


    }
}
