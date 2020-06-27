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
                
                var Orders = db.Orders.ToList();
                foreach (var order in Orders)
                {
                    order.OrderElements = db.OrderElements
                        .Where(x => x.GrowStart.Date >= DateTime.Now.Date || x.GrowStart == null)
                        .Where(x => x.OrderId == order.Id)
                        .ToList();

                    foreach (var orderElement in order.OrderElements)
                    {
                        orderElement.item = db.Items.First(x => x.Id == orderElement.ItemId);
                        orderElement.UserName = order.UserName;
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

            OrdersModel Model = new OrdersModel();

            var Today = OrderList.Where(x => x.Created.Date == DateTime.Now.Date).ToList();
            var Yesterday = OrderList.Where(x => x.Created.Date == DateTime.Now.Date.AddDays(-1)).ToList();
            var Before = OrderList.Where(x => x.Created.Date < DateTime.Now.Date.AddDays(-1)).ToList();

            Model.Today = Today;
            Model.Yesterday = Yesterday;
            Model.Before = Before;

            return View(Model);
        }

        [HttpGet]
        public IActionResult CreateOrder()
        {
            var Order = new Order();
            return View(Order);
        }

        [HttpPost]
        public IActionResult CreateOrder(Order order)
        {
            using(Database db = new Database())
            {
                order.Created = DateTime.Now;
                order.Status = "Не начат";
                db.Orders.Add(order);
                db.SaveChanges();
            }

            return RedirectToAction("Order", new { OrderId = order.Id});
        }

        public IActionResult Order(int OrderId)
        {
            var OrderModel = new OrderModel();
            var Order = new Order();
            using(Database db = new Database())
            {
                Order = db.Orders.First(x => x.Id == OrderId);
                Order.OrderElements = db.OrderElements.Where(x => x.OrderId == OrderId).ToList();
                foreach (var orderEl in Order.OrderElements)
                {
                    orderEl.item = db.Items.First(x => x.Id == orderEl.ItemId);
                }

                OrderModel.Items = db.Items.ToList();
            }

            OrderModel.Order = Order;
            var GrowsElements = Order.OrderElements.Where(x => x.isGrow == true).ToList();
            if(GrowsElements.Count != 0)
                OrderModel.Grows.AddRange(GrowsElements);

            var NotGrowsElements = Order.OrderElements.Where(x => x.isGrow != true).ToList();
            if (NotGrowsElements.Count != 0)
                OrderModel.NotGrows.AddRange(NotGrowsElements);

            return View(OrderModel);
        }

        public IActionResult AddElementToOrder(OrderModel orderModel)
        {
            using(Database db = new Database())
            {
                db.OrderElements.Add(new OrderElement
                {
                    OrderId = orderModel.Order.Id,
                    ItemId = orderModel.ItemId,
                     Count = orderModel.Amount
                });
                db.SaveChanges();
            }

            return RedirectToAction("Order", new { OrderId = orderModel.Order.Id });
        }

        public IActionResult StartGrowOrder(Order order)
        {
            using (Database db = new Database())
            {
                order = db.Orders.First(x => x.Id == order.Id);
                order.OrderElements = db.OrderElements.Where(x => x.OrderId == order.Id).ToList();
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

                order.Status = "В процессе";
                db.SaveChanges();
            }

            return RedirectToAction("Orders");
        }

        public IActionResult StartGrowElement(OrderElement element)
        {
            using(Database db = new Database())
            {
                element = db.OrderElements.First(x => x.Id == element.Id);
                element.isGrow = true;
                element.GrowStart = DateTime.Now;
                db.SaveChanges();
            }
            return RedirectToAction("Calendar");
        }

        public IActionResult Grow()
        {
            List<OrderElement> orderElements = new List<OrderElement>();
            using(Database db =new Database())
            {
                orderElements = db.OrderElements.Where(x => x.isGrow == true).OrderByDescending(x => x.GrowStart).ToList();
                foreach (var element in orderElements)
                {
                    element.item = db.Items.FirstOrDefault(x => x.Id == element.ItemId);
                    element.UserName = db.Orders.FirstOrDefault(x => x.Id == element.OrderId).UserName;
                }
            }
            return View(orderElements);
        }

    }
}
