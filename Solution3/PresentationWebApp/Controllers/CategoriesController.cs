using Microsoft.AspNetCore.Mvc;
using ShoppingCart.Application.Interfaces;
using ShoppingCart.Application.ViewModels;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace PresentationWebApp.Controllers
{
    public class CategoriesController : Controller
    {
        private readonly IProductsService _productsService;
        private readonly ICategoriesService _categoriesService;
        public CategoriesController(IProductsService productsService, ICategoriesService categoryService)
        {
            _productsService = productsService;
            _categoriesService = categoryService;
        }
        public IActionResult Index()
        {
            var list = _categoriesService.GetCategories();
            return View(list);
        }
        [HttpGet]
        public IActionResult Create()
        {
            return View();
        }
        [HttpPost]
        public IActionResult Create(CategoryViewModel data)
        {
            try
            {
                _categoriesService.AddCategory(data);
                ViewData["feedback"] = "Category was added successfully";
            }
            catch (Exception e)
            {
                ViewData["warning"] = "Category was not added !" + e.Message;

            }
            return View(data);
        }
    }
}
