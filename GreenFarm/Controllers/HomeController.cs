using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Logging;
using GreenFarm.Models;
using Microsoft.AspNetCore.Authorization;
using GreenFarm.ViewModels;

namespace GreenFarm.Controllers
{

    public class HomeController : Controller
    {
        private readonly ILogger<HomeController> _logger;

        public HomeController(ILogger<HomeController> logger)
        {
            _logger = logger;
        }
        
        public IActionResult Index()
        {
            HomeModel model = new HomeModel();
            using(Database db = new Database())
            {
                model.User = db.Users.FirstOrDefault(x => x.PhoneNumber == HttpContext.User.Identity.Name);
            }

            return View("Index",model);
        }

        public IActionResult Privacy()
        {
            return View();
        }

        [ResponseCache(Duration = 0, Location = ResponseCacheLocation.None, NoStore = true)]
        public IActionResult Error()
        {
            return View(new ErrorViewModel { RequestId = Activity.Current?.Id ?? HttpContext.TraceIdentifier });
        }

        public IActionResult Calendar()
        {
            CalendarModel model = new CalendarModel();
            using(Database db = new Database())
            {
                
                var Orders = db.Orders.Where(x => x.isClosed == false).ToList();
                foreach (var order in Orders)
                {
                    order.OrderElements = db.OrderElements
                        .Where(x => x.GrowStart.Date >= DateTime.Now.Date || x.GrowStart == null)
                        .Where(x => x.OrderId == order.Id)
                        .ToList();

                    foreach (var orderElement in order.OrderElements)
                    {
                        orderElement.item = db.Items.First(x => x.Id == orderElement.ItemId);
                    }

                    
                    model.Today.AddRange(order.OrderElements.Where(x => x.GrowStart.Date == DateTime.Now.Date));
                    model.Tomorrow.AddRange(order.OrderElements.Where(x => x.GrowStart.Date == DateTime.Now.Date.AddDays(1)));
                    model.DayAfterTomorrow.AddRange(order.OrderElements.Where(x => x.GrowStart.Date == DateTime.Now.Date.AddDays(2)));
                    model.Next.AddRange(order.OrderElements.Where(x => x.GrowStart.Date > DateTime.Now.Date.AddDays(2)));
                }
            }

            return View(model);
        }

        public IActionResult Orders()
        {
            List<Order> OrderList = new List<Order>();
            using(Database db = new Database())
            {
                OrderList = db.Orders.ToList();
                foreach (var order in OrderList)
                {
                    order.OrderElements = db.OrderElements.Where(x => x.OrderId == order.Id).ToList();
                    foreach (var orderElement in order.OrderElements)
                    {
                        orderElement.item = db.Items.First(x => x.Id == orderElement.ItemId);
                    }
                }
            }
            return View(OrderList);
        }

        public IActionResult StartGrowOrder(int OrderId)
        {
            var order = new Order();
            using (Database db = new Database())
            {
                order = db.Orders.First(x => x.Id == OrderId);
                order.OrderElements = db.OrderElements.Where(x => x.OrderId == OrderId).ToList();
                foreach (var orderElement in order.OrderElements)
                {
                    orderElement.item = db.Items.First(x => x.Id == orderElement.ItemId);
                }


                int maxGrowDays = order.OrderElements.Max(x => x.item.GrowDays);
                foreach (var orderElement in order.OrderElements)
                {
                    orderElement.GrowStart = DateTime.Now.AddDays(maxGrowDays - orderElement.item.GrowDays);
                    orderElement.HarvestDate = orderElement.GrowStart.AddDays(orderElement.item.GrowDays);
                }
                db.SaveChanges();
            }

            return RedirectToAction("Orders");
        }

        public IActionResult Grow(int orderElementId)
        {
            using(Database db = new Database())
            {
                var element = db.OrderElements.First(x => x.Id == orderElementId);
                element.isGrow = true;
                db.SaveChanges();
            }
            return RedirectToAction("Calendar");
        }

    }
}
