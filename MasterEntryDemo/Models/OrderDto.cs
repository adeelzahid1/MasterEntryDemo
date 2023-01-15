using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace MasterEntryDemo.Models
{
    public class OrderDto
    {
        public System.Guid CustomerId { get; set; }
        public string Name { get; set; }
        public string Address { get; set; }
        public System.DateTime OrderDate { get; set; }

        public ICollection<Order> Orders { get; set; }
    }
}