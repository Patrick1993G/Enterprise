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
            var ids = list.Split(",").ToList();
          
            List<ProductViewModel> products = new List<ProductViewModel>();

            //search for duplicates
            //IEnumerable<String> duplicateIds = idList.GroupBy(x => x)
            //                        .Where(y => y.Count() > 1)
            //                        .Select(x => x.Key);

            foreach (var item in ids)
            {
                if (!String.IsNullOrEmpty(item)) // if item is not empty
                {
                    var product = _productsService.GetProduct(new Guid(item)); // get the product by id
                    //check if there is a same id
                    if (product.quantity >0)
                    {
                        product.quantity++; // increment the quantity
                        products.Add(product);// add it to the list
                        int index =ids.FindIndex(idx => idx.Equals(item));
                        ids[index] = " ";
                    }
                    else
                    {
                        products.Add(_productsService.GetProduct(new Guid(item)));
                    }
                }
            }
            return View(products);
        }
    }
}
