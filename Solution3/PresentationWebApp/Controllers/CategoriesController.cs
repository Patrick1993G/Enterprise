using Microsoft.AspNetCore.Authorization;
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
                TempData["feedback"] = "Category was added successfully";
            }
            catch (Exception e)
            {
                TempData["warning"] = "Category was not added !" + e.Message;

            }
            return View(data);
        }
        [Authorize(Roles = "Admin")]
        public IActionResult Delete(int id)
        {
            try
            {
                if (id <= 0)
                {
                    TempData["warning"] = "Incorrect Category ID !";
                }
                else
                {
                    _categoriesService.DeleteCategory(id);
                    TempData["feedback"] = "Category was deleted successfully";
                }

            }
            catch (Exception e)
            {
                TempData["warning"] = "Category was not deleted !" + e.Message;

            }
            return RedirectToAction("Index");
        }
        [Authorize(Roles = "Admin")]
        public IActionResult Disable(int id)
        {
            try
            {
                if (id <=0)
                {
                    TempData["warning"] = "Incorrect Category ID !";
                }
                {
                    _categoriesService.DisableCategory(id);
                    TempData["feedback"] = "Category was disabled successfully";
                }

            }
            catch (Exception e)
            {
                TempData["warning"] = "Category was not disabled !" + e.Message;

            }
            return RedirectToAction("Index");
        }
    }
}
