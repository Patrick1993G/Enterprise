using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using ShoppingCart.Application.Interfaces;
using ShoppingCart.Application.ViewModels;
using ShoppingCart.Domain.Models;

namespace PresentationWebApp.Controllers
{
    public class ProductsController : Controller
    {
     
        private readonly IProductsService _productsService;
        private readonly ICategoriesService _categoriesService;
        private IWebHostEnvironment _environment;
        public ProductsController(IProductsService productsService,ICategoriesService categoryService, IWebHostEnvironment environment)
        {
            _productsService = productsService;
            _categoriesService = categoryService;
            _environment = environment;
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
        [Authorize(Roles = "Admin")]
        public IActionResult Delete(Guid id)
        {
            try
            {
                if (id == null)
                {
                    TempData["warning"] = "Incorrect Product ID !";
                }
                else
                {
                    _productsService.DeleteProduct(id);
                    TempData["feedback"] = "Product was deleted successfully";
                }
                
            }
            catch (Exception e)
            {
                TempData["warning"] = "Product was not deleted !" + e.Message;

            }
            return RedirectToAction("Index");
        }
        public IActionResult Disable(Guid id)
        {
            try
            {
                if (id == null)
                {
                    TempData["warning"] = "Incorrect Product ID !";
                }
                {
                    _productsService.DisableProduct(id);
                    TempData["feedback"] = "Product was disabled successfully";
                }
               
            }
            catch (Exception e)
            {
                TempData["warning"] = "Product was not disabled !" + e.Message;

            }
            return RedirectToAction("Index");
        }
        [HttpGet]
        [Authorize (Roles ="Admin")]
        public IActionResult Create()
        {
            RefreshInfo();
            return View();
        }
        [HttpPost]
        [Authorize(Roles = "Admin")]
        public IActionResult Create(ProductViewModel data, IFormFile file)
        {
            try
            {
                if (file != null)
                {
                    if (file.Length >0)
                    {
                        string newFilename = Guid.NewGuid() + System.IO.Path.GetExtension(file.FileName);
                        string newFilenameWithAbsolutePath = _environment.WebRootPath + @"\Images\" + newFilename;
                        using (var stream = System.IO.File.Create(newFilenameWithAbsolutePath))
                        {
                            file.CopyTo(stream);
                        }

                        data.ImageUrl = @"\Images\" + newFilename;

                    }

                }
                _productsService.AddProduct(data);
                TempData["feedback"] = "Product was added successfully";
            }
            catch (Exception e)
            {
                TempData["warning"] = "Product was not added !"+e.Message;

            }
            RefreshInfo();
            return View(data);
        }

        //helpers
        private void RefreshInfo()
        {
            //fetch a list of categories
            var listOfCategories = _categoriesService.GetCategories();
            var listOFProducts = _productsService.GetProducts();
            //pass to the view
            ViewBag.Products = listOFProducts;
            ViewBag.Categories = listOfCategories;
        }
    }
}
