using System;
using System.Collections.Generic;
using GreenFarm.Models;

namespace GreenFarm.ViewModels
{
    public class CartModel
    {
        public CartModel()
        {
            OrderList = new List<OrderElement>();
        }

        public string UserName { get; set; }
        public List<OrderElement> OrderList { get; set; }
        public decimal TotalPrice { get; set; }
        public string PaymentMethod { get; set; }
    }
}
