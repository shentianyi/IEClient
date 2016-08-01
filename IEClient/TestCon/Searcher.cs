using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace TestCon
{
    public class People {
        public string Name { get; set; }
        public int Age { get; set; }

        public override string ToString() {
            return string.Format("name:{0} ,age:{1}", this.Name, this.Age);
        }
    }
  public  class Searcher
    {
        public static List<People> FindByName(string name)
        {
            return GetData().Where(p => p.Name.Contains(name)).ToList();
        }

        public static List<People> FindBetAge(int minAge, int maxAge) {
            return GetData().Where(p => p.Age > minAge && p.Age < maxAge && p.Name.Contains("1")).ToList();
        }

        public static List<People> GetData() {
            List<People> peoples = new List<People>();
            for (int i = 0; i < 100; i++) {
                peoples.Add(new People() { Name = i.ToString(), Age = i });

            }
            return peoples;
        }
    }
}
