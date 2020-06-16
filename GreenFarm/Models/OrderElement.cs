using System;
using System.ComponentModel.DataAnnotations.Schema;

namespace GreenFarm.Models
{
    public class OrderElement
    {
        public OrderElement()
        {
        }

        public int Id { get; set; }
        public int OrderId { get; set; }
        [NotMapped]
        public string UserName { get; set; }
        public int ItemId { get; set; }
        public Item item { get; set; }
        public int Count { get; set; }
        public DateTime GrowStart { get; set; }
        public DateTime HarvestDate { get; set; }
        public bool isGrow { get; set; }

    }
}
