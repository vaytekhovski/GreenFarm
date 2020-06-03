using System;
using System.Collections.Generic;
using GreenFarm.Models;

namespace GreenFarm.ViewModels
{
    public class CartModel
    {
        public CartModel()
        {
            OrderList = new List<Order>();
        }

        public List<Order> OrderList { get; set; }
        public decimal TotalPrice { get; set; }
        public string PaymentMethod { get; set; }
    }
}
