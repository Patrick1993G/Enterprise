using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Newtonsoft.Json;
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
            List<ProductViewModel> products = GetProducts();
            return View(products);
        }
        public IActionResult Remove(Guid id)
        {
            List<ProductViewModel> products = GetProducts();
            var product = _productsService.GetProduct(id);
            var productToAlter =products.FirstOrDefault(x => x.Id == product.Id);
            productToAlter.Quantity--;
            if (productToAlter.Quantity == 0)
            {
                products.Remove(productToAlter);
            }
            TempData["feedback"] = ("Product was removed successfully");
            HttpContext.Session.SetString(SessionKeyName, ParseSessionListToString(products));
            return View("Index", products);
        }
        private String ParseSessionListToString(List<ProductViewModel> products)
        {
            String toReturn = "";
            foreach (var item in products)
            {
                if (products.Count() > 0)
                {
                    toReturn += JsonConvert.SerializeObject(item) + "/";
                }

            }
            return toReturn;
        }
        private List<ProductViewModel> ParseSessionStringToList()
        {
            List<ProductViewModel> products = new List<ProductViewModel>();
            string data = HttpContext.Session.GetString(SessionKeyName);
            if (data != null)
            {
                List<String> stringList = data.Split('/').ToList();
                foreach (var item in stringList)
                {
                    if (!String.IsNullOrEmpty(item))
                    {
                        products.Add(JsonConvert.DeserializeObject<ProductViewModel>(item));
                    }

                }
            }

            return products;

        }
        private List<ProductViewModel> GetProducts()
        {
            var products = ParseSessionStringToList();
            return products;
        }
    }
}
