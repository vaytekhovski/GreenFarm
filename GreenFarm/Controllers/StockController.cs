using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Threading.Tasks;
using GreenFarm.Models;
using GreenFarm.ViewModels;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;


// For more information on enabling MVC for empty projects, visit https://go.microsoft.com/fwlink/?LinkID=397860

namespace GreenFarm.Controllers
{
    public class StockController : Controller
    {
        public IActionResult Index()
        {
            StockModel model = new StockModel();
            using(Database db = new Database())
            {
                model.ItemList = db.Items.ToList();
            }
            return View(model);
        }

        [HttpGet]
        public IActionResult CreateItem()
        {
            Item item = new Item();
            return View(item);
        }

        [HttpPost]
        public async Task<IActionResult> CreateItem(Item item)
        {
            using(Database db = new Database())
            {

                if (item.ImageFile != null)
                {
                    string fileName = Path.GetFileNameWithoutExtension(item.ImageFile.FileName);
                    string extension = Path.GetExtension(item.ImageFile.FileName);
                    fileName = fileName + DateTime.Now.ToString("yymmssfff") + extension;

                    item.ImagePath = "/img/" + fileName;
                    fileName = Path.Combine(Directory.GetCurrentDirectory() + "/wwwroot/img/", fileName);
                    item.ImageFile.CopyTo(new FileStream(fileName, FileMode.Create));

                }

                db.Items.Add(item);
                await db.SaveChangesAsync();

            }

            return RedirectToAction("Index", "Stock");
        }

    }
}
