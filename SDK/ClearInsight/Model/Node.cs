using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Linq;
using System.Text;

namespace ClearInsight.Model
{
    public class Node:INotifyPropertyChanged
    {
        public event PropertyChangedEventHandler PropertyChanged;
        public void OnPropertyChanged(PropertyChangedEventArgs e)
        {
            if (PropertyChanged != null)
            {
                PropertyChanged(this, e);
            }
        }

        private string _devise_code;

        public int id { get; set; }
        public int type { get; set; }
        public int node_set_id { get; set; }
        public int tenant_id { get; set; }

        public string name { get; set; }
        public string code { get; set; }
        public string uuid { get; set; }
        public string devise_code
        {
            get { return _devise_code; }
            set
            {
                _devise_code = value;
                OnPropertyChanged(new PropertyChangedEventArgs("devise_code"));
            }
        }
        public bool is_selected { get; set; }
        public string ancestry { get; set; }

        public DateTime created_at { get; set; }
        public DateTime updated_at { get; set; }

    }
}
