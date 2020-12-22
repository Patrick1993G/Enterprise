using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using ShoppingCart.Application.Interfaces;
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
            var list = _orderDetailsService.GetOrderDetails();
            return View(list);
        }

        
    }
}
