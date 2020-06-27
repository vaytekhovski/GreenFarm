using System;
using System.Collections.Generic;
using GreenFarm.Models;

namespace GreenFarm.ViewModels
{
    public class OrdersModel
    {
        public OrdersModel()
        {
            Today = new List<Order>();
            Yesterday = new List<Order>();
            Before = new List<Order>();
        }

        public List<Order> Today { get; set; }
        public List<Order> Yesterday { get; set; }
        public List<Order> Before { get; set; }
    }
}
