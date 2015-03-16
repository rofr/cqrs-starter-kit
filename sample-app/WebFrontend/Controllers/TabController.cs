using System;
using System.Collections.Generic;
using System.Linq;
using System.Web.Mvc;
using Cafe;
using WebFrontend.ActionFilters;
using WebFrontend.Models;
using System.Text.RegularExpressions;

namespace WebFrontend.Controllers
{
    [IncludeLayoutData]
    public class TabController : Controller
    {
        public ActionResult Open()
        {
            return View();
        }

        [HttpPost]
        public ActionResult Open(OpenTabModel model)
        {
            var cmd = new OpenTab
            {
                Id = Guid.NewGuid(), 
                Waiter = model.Waiter, 
                TableNumber = model.TableNumber
            };
            Domain.Dispatcher.SendCommand(cmd);
            return RedirectToAction("Order", new { id = cmd.TableNumber });
        }
        
        public ActionResult Status(int id)
        {
            return View(Domain.OpenTabQueries.TabForTable(id));
        }

        public ActionResult Order(int id)
        {
            return View(new OrderModel
            {
                Items = (from item in StaticData.Menu
                         select new OrderModel.OrderItem
                         {
                             MenuNumber = item.MenuNumber,
                             Description = item.Description,
                             NumberToOrder = 0
                         }).ToList(),
            });
        }

        [HttpPost]
        public ActionResult Order(int id, OrderModel order)
        {
            var items = new List<TabItem>();
            var menuLookup = StaticData.Menu.ToDictionary(k => k.MenuNumber, v => v);
            foreach (var item in order.Items)
                for (var i = 0; i < item.NumberToOrder; i++)
                {
                    var tabItem = menuLookup[item.MenuNumber].IsDrink ? (TabItem) new Drink() : new FoodItem();
                    tabItem.MenuNumber = item.MenuNumber;
                    tabItem.Description = menuLookup[item.MenuNumber].Description;
                    tabItem.Price = menuLookup[item.MenuNumber].Price;
                    items.Add(tabItem);
                }
            
            Domain.Dispatcher.SendCommand(new PlaceOrder
            {
                Id = Domain.OpenTabQueries.TabIdForTable(id),
                Items = items
            });

            return RedirectToAction("Status", new { id = id });
        }


        public ActionResult MarkServed(int id, FormCollection form)
        {
            var tabId = Domain.OpenTabQueries.TabIdForTable(id);
            var menuNumbers = (from entry in form.Keys.Cast<string>()
                               where form[entry] != "false"
                               let m = Regex.Match(entry, @"served_\d+_(\d+)")
                               where m.Success
                               select int.Parse(m.Groups[1].Value)
                              ).ToList();

            var menuLookup = StaticData.Menu.ToDictionary(k => k.MenuNumber, v => v);
            
            var drinks = menuNumbers.Where(n => menuLookup[n].IsDrink).ToList();
            if (drinks.Any())
                Domain.Dispatcher.SendCommand(new MarkDrinksServed
                {
                    Id = tabId,
                    MenuNumbers = drinks
                });

            var food = menuNumbers.Where(n => !menuLookup[n].IsDrink).ToList();
            if (food.Any())
                Domain.Dispatcher.SendCommand(new MarkFoodServed
                {
                    Id = tabId,
                    MenuNumbers = food
                });

            return RedirectToAction("Status", new { id = id });
        }

    }
}
