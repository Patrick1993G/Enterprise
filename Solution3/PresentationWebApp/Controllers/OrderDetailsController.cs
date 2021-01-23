using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using ShoppingCart.Application.Interfaces;
using ShoppingCart.Application.ViewModels;
using ShoppingCart.Domain.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace PresentationWebApp.Controllers
{
    public class OrderDetailsController : Controller
    {
        const string SessionKeyName = "_Basket";
        private readonly IOrdersService _ordersService;
        private readonly IProductsService _productsService;
        private readonly IOrderDetailsService _orderDetailsService;
        private IWebHostEnvironment _environment;
        public OrderDetailsController(IOrdersService ordersService, IProductsService productsService, IOrderDetailsService orderDetailsService, IWebHostEnvironment environment)
        {
            _productsService = productsService;
            _ordersService = ordersService;
            _orderDetailsService = orderDetailsService;
            _environment = environment;
        }

        [Authorize(Roles = "Admin")]
        public IActionResult Index()
        {
            var list = _orderDetailsService.GetOrderDetails().ToList();
            return View(list);
        }
        private bool StockManagment(Guid id)
        { 
            //get all products in the order
            var list = _orderDetailsService.GetOrderDetails().Where(x => x.Order.Id == id).ToList();
            foreach (var item in list)
            {
                for (int i = 0; i < item.Quantity; i++)
                {
                    _productsService.IncreaseStock(item.Product.Id);
                }
            }
            return true;
        }
        public  IActionResult Delete(Guid id)
        {
            //manage the stock
            StockManagment(id);
            _ordersService.DeleteOrder(id);
            TempData["feedback"] = ("Order was removed successfully");
            return RedirectToAction("Index");
        }
        
    }
}
