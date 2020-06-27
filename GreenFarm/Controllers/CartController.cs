using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using GreenFarm.Models;
using GreenFarm.ViewModels;
using Microsoft.AspNetCore.Mvc;

// For more information on enabling MVC for empty projects, visit https://go.microsoft.com/fwlink/?LinkID=397860

namespace GreenFarm.Controllers
{
    public class CartController : Controller
    {
        [HttpGet]
        public IActionResult Index()
        {
            CartModel model = new CartModel();
            List<Item> Items = new List<Item>();
            using (Database db = new Database())
            {
                Items = db.Items.ToList();
            }

            foreach (var item in Items)
            {
                model.OrderList.Add(new OrderElement
                {
                    item = item,
                    Count = 1
                });
            }
            return View(model);
        }

        [HttpPost]
        public IActionResult Index(CartModel model)
        {
            return View();
        }

        [HttpGet]
        public IActionResult CreateOrder()
        {
            return View();
        }

        [HttpPost]
        public IActionResult CreateOrder(CartModel cart)
        {
            using (Database db = new Database())
            {
                var Order = new Order()
                {
                    Created = DateTime.Now,
                    OrderElements = cart.OrderList,
                    UserName = cart.UserName
                };
                db.Orders.Add(Order);
                db.SaveChanges();
                var OrderId = db.Orders.Last(x => x.UserName == cart.UserName).Id;

                foreach (var item in cart.OrderList)
                {
                    item.OrderId = OrderId;
                    db.OrderElements.Add(item);
                }
                db.SaveChanges();

            }
            return View(cart);
        }

    }
}
