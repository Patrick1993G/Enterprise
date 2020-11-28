using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using ShoppingCart.Application.Interfaces;
using ShoppingCart.Application.ViewModels;

namespace PresentationWebApp.Controllers
{
    public class ProductsController : Controller
    {
     
        private readonly IProductsService _productsService;
        private readonly ICategoriesService _categoriesService;
        public ProductsController(IProductsService productsService,ICategoriesService categoryService)
        {
            _productsService = productsService;
            _categoriesService = categoryService;
        }
        public IActionResult Index()
        {
            var list = _productsService.GetProducts();
            return View(list);
        }

        public IActionResult Details(Guid id)
        {
            var product = _productsService.GetProduct(id);
            return View(product);
        }
        [HttpGet]
        public IActionResult Delete()
        {
            RefreshInfo();
            return View();
        }
        [HttpPost]
        public IActionResult Delete(ProductViewModel data)
        {
            try
            {
                _productsService.DeleteProduct(data);
                ViewData["feedback"] = "Product was deleted successfully";
            }
            catch (Exception e)
            {
                ViewData["warning"] = "Product was not deleted !" + e.Message;

            }
            RefreshInfo();
            return View(data);
        }

        [HttpGet]
        public IActionResult Create()
        {
            RefreshInfo();
            return View();
        }
        [HttpPost]
        public IActionResult Create(ProductViewModel data)
        {
            try
            {
                _productsService.AddProduct(data);
                ViewData["feedback"] = "Product was added successfully";
            }
            catch (Exception e)
            {
                ViewData["warning"] = "Product was not added !"+e.Message;

            }
            RefreshInfo();
            return View(data);
        }

        //helpers
        private void RefreshInfo()
        {
            //fetch a list of categories
            var listOfCategories = _categoriesService.GetCategories();
            //pass to the view
            ViewBag.Categories = listOfCategories;
        }
    }
}
