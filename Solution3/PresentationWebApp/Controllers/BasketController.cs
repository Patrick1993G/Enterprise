using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using ShoppingCart.Application.Interfaces;
using ShoppingCart.Application.ViewModels;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace PresentationWebApp.Controllers
{
    public class BasketController : Controller
    {
        const string SessionKeyName = "_Basket";

        private readonly IProductsService _productsService;

        public BasketController(IProductsService productsService)
        {
            _productsService = productsService;
        }

        public IActionResult Index()
        {
            var list = HttpContext.Session.GetString(SessionKeyName);
            var ids = list.Split(",");
            List<ProductViewModel> products = new List<ProductViewModel>();
            foreach (var item in ids)
            {
                if (!String.IsNullOrEmpty(item))
                {
                    products.Add(_productsService.GetProduct(new Guid(item)));
                }
               
            }
            return View(products);
        }
    }
}
