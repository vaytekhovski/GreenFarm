using System;
using System.ComponentModel.DataAnnotations.Schema;
using Microsoft.AspNetCore.Http;

namespace GreenFarm.Models
{
    public class Item
    {
        public Item()
        {

        }
        public int Id { get; set; }
        [NotMapped]
        public IFormFile ImageFile { get; set; }
        public string ImagePath { get; set; }
        public string Name { get; set; }
        public string Description { get; set; }
        public decimal Price { get; set; }
        public bool Available { get; set; }

    }
}
