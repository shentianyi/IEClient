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
using ClearInsight;
using ClearInsight.Model;
using IEClient.Properties;
using IEClient.Config;

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
            //this.WindowState = System.Windows.WindowState.Maximized;
            //this.WindowStyle = System.Windows.WindowStyle.None;
            LoadData();
        }

        private void LoadData()
        {
            ClearInsightAPI ci = new ClearInsightAPI(BaseConfig.Server, UserSession.GetInstance().CurrentUser.token);
            List<Project> projects = ci.GetProjects();
            this.UniformGrid.DataContext = projects;
        }
       
        private void to_Check_Cleck(object sender, RoutedEventArgs e)
        {
            if (this.UniformGrid.SelectedIndex > -1)
            {
                Project project = this.UniformGrid.SelectedItem as Project;
                UserSession.GetInstance().CurrentProject = project;
                CheckWindow win = new CheckWindow();
                win.Show();
                this.Close();
            }
        }
    }
}
