using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace ClearInsight.Model
{
    public class UserSession
    {
        static UserSession instance;
        static object locker = new object();
        public User CurrentUser { get; set; }

        public static UserSession GetInstance()
        {
            if (instance == null)
            {
                lock (locker)
                {
                    if (instance == null)
                    {
                        instance = new UserSession();
                    }
                }
            }
            return instance;
        }
    }
}
