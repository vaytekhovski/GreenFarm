using System;
namespace GreenFarm.Models
{
    public class Order
    {
        public Order()
        {
        }

        public Item item { get; set; }
        public int Count { get; set; }
    }
}
