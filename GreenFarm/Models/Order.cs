using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations.Schema;
using MySql.Data.EntityFrameworkCore.DataAnnotations;

namespace GreenFarm.Models
{
    public class Order
    {
        public Order()
        {
            OrderElements = new List<OrderElement>();
        }

        public int Id { get; set; }
        public int UserId { get; set; }
        public string UserName { get; set; }
        public DateTime Created { get; set; }
        public List<OrderElement> OrderElements { get; set; }
        public string Status { get; set; }
    }
}
