using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DemoClient
{
   public class CustomerInfo 
    {
        public CustomerInfo()
        {

        }

        public CustomerInfo(string id, string name)
        {
            Id = id;
            Name = name;
        }
        public string Id { get; set; }
        public string Name { get; set; }
    }
}
