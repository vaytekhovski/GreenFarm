using System;
using System.Collections.Generic;

namespace GreenFarm.Models
{
    public class Order
    {
        public Order()
        {
        }

        public int Id { get; set; }
        public int UserId { get; set; }
        public string UserName { get; set; }
        public DateTime Created { get; set; }
        public List<OrderElement> OrderElements { get; set; }
        public bool isClosed { get; set; }
    }
}
