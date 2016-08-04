using ClearInsight.Model;
using IEClientLib;
using MahApps.Metro.Controls;
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
    /// DetailPage.xaml 的交互逻辑
    /// </summary>
    public partial class DetailWindow : MetroWindow
    {
        public IESlave<Node> Slave { get; set; }

        public DetailWindow()
        {
            InitializeComponent();
            
        }

        private void MetroWindow_Loaded(object sender, RoutedEventArgs e)
        { 
            //List<IEData<Node>> data = new List<IEData<Node>>();
            //for(int i = 0; i < 1000; i++)
            //{
            //    data.Add(new IEData<Node> { Time=new Random().Next(100),PolledAt=DateTime.Now });
            //}
            this.DataListDG.ItemsSource =this.Slave.DataList;
 
            this.Title = Slave.Name;
           // List<IEData<Node>> data = new List<IEData<Node>>();
           // for(int i = 0; i < 1000; i++)
           // {
            //    data.Add(new IEData<Node> { LineNumber=i+1, Time=new Random().Next(100),PolledAt=DateTime.Now });
           // }
           // this.DataListDG.ItemsSource = data;//this.Slave.DataList;
            listCount.Text = this.Slave.DataList.Count.ToString();
 
        }
    }
}
