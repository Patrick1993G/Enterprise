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
            var groupedIds = from id in ids
                             group id by id into g
                             let count = g.Count()
                             orderby count ascending
                             select new { value = g.Key, count = count };
            List<ProductViewModel> products = new List<ProductViewModel>();

            //search for duplicates
            //IEnumerable<String> duplicateIds = idList.GroupBy(x => x)
            //                        .Where(y => y.Count() > 1)
            //                        .Select(x => x.Key);
            foreach (var item in groupedIds)
            {
                if (!String.IsNullOrEmpty(item.value))
                {
                    var product = _productsService.GetProduct(new Guid(item.value));
                    product.quantity = item.count;
                    products.Add(product);
                }
            }
            return View(products);
        }
    }
}
