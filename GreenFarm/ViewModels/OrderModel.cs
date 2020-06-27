using System;
using System.Collections.Generic;
using GreenFarm.Models;

namespace GreenFarm.ViewModels
{
    public class OrderModel
    {
        public OrderModel()
        {
            Grows = new List<OrderElement>();
            NotGrows = new List<OrderElement>();
        }

        public Order Order { get; set; }

        public List<Item> Items { get; set; }

        public int ItemId { get; set; }
        public int Amount { get; set; }

        public List<OrderElement> Grows { get; set; }
        public List<OrderElement> NotGrows { get; set; }
    }
}
