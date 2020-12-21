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
        IList<ProductViewModel> GetPage(IQueryable<ProductViewModel> list, int page, int pageSize)
        {
            return list.Skip(page * pageSize).Take(pageSize).ToList();
        }
        public IActionResult Index(int page =1, int pageSize = 4)
        {
            RefreshInfo();
            var list = _productsService.GetProducts();
            int listCount = list.Count();
            IList<ProductViewModel> firstPage = GetPage(list, 0, 10);
            return View(firstPage);
        }
        public IActionResult GetNextPage(int currentPage) {
            var list = _productsService.GetProducts();
            IList<ProductViewModel> nextPage = GetPage(list,currentPage*10,10);
            return View(nextPage);
        }
        public IActionResult Filter(int id)
        {
            RefreshInfo();
            var list = _productsService.GetProducts(id);
            // return RedirectToAction("Index", list);
            return View("Index", list);
        }
        [HttpPost]
        public IActionResult Search(string keyword)
        {
            var list = _productsService.GetProducts(keyword).ToList();
            return View("Index", list);
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
        [Authorize(Roles = "Admin")]
        public IActionResult Disable(Guid id)
        {
            var product = _productsService.GetProduct(id);
            try
            {
                if (id == null)
                {
                    TempData["warning"] = "Incorrect Product ID !";
                }
                {
                    _productsService.DisableProduct(id);
                    if (!product.Disable)
                    {
                        TempData["feedback"] = "Product was disabled successfully";
                    }
                    else
                    {
                        TempData["feedback"] = "Product was enabled successfully";
                    }
                    
                }
               
            }
            catch (Exception e)
            {
                if (!product.Disable)
                {
                    TempData["warning"] = "Product was not disabled !" + e.Message;

                }
                else
                {
                    TempData["warning"] = "Product was not enabled !" + e.Message;

                }

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
            //pass to the view
            ViewBag.Categories = listOfCategories;
        }
    }
}
