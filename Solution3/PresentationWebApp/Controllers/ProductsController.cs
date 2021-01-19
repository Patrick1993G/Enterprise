using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Newtonsoft.Json;
using ShoppingCart.Application.Interfaces;
using ShoppingCart.Application.ViewModels;
using ShoppingCart.Domain.Models;
namespace PresentationWebApp.Controllers
{
    public class ProductsController : Controller
    {
        const string SessionKeyName = "_Basket";
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
        public IActionResult Index()
        {
            RefreshInfo();
            var list = _productsService.GetProducts();
            IList<ProductViewModel> firstPage = GetPage(list, 0, 10);
            ViewBag.pageNo = 1;
            return View(firstPage);
        }
        public IActionResult GetNextPage(int currentPage) {
            RefreshInfo();
            if (currentPage <= Convert.ToInt32(ViewBag.pageNo))
            {
                ViewBag.pageNo = Convert.ToInt32(ViewBag.pageNo) + 1;
            }
            var list = _productsService.GetProducts();
            IList<ProductViewModel> nextPage = GetPage(list,currentPage,10);
            
            return View("Index", nextPage);
        }
        public IActionResult GetPreviousPage(int currentPage)
        {
            RefreshInfo();
            if (currentPage != 0)
            {
                ViewBag.pageNo = Convert.ToInt32(ViewBag.pageNo) - 1;
            }
            var list = _productsService.GetProducts();
            IList<ProductViewModel> previousPage = GetPage(list, currentPage, 10);
            
            return RedirectToAction("Index", previousPage);
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
        private String ParseSessionListToString(List<ProductViewModel> products)
        {
            String toReturn = "";
            foreach (var item in products)
            {
                if (products.Count() >0)
                {
                    toReturn += JsonConvert.SerializeObject(item)+"/";
                }

            }
            return toReturn;
        }
        public IActionResult Add(Guid id)
        {
            ProductViewModel toAdd = _productsService.GetProduct(id);
            List <ProductViewModel> products = ParseSessionStringToList();
            ProductViewModel toCheck = products.FirstOrDefault(x => x.Id == toAdd.Id);
            if (toCheck != null)
            {
                toCheck.Quantity++;
                TempData["feedback"] = ("Product was added again successfully");
            }
            else
            {
                products.Add(toAdd);
                TempData["feedback"] = ("Product was added successfully");
            }
            //decrease the stock
            _productsService.DecreaseStock(id);
            HttpContext.Session.SetString(SessionKeyName,ParseSessionListToString(products));
            RefreshInfo();
            return RedirectToAction("Index");
        }
        public IActionResult Filter(int id)
        {
            RefreshInfo();
            var list = _productsService.GetProducts(id);
            return View("Index", list);
        }
        [HttpPost]
        public IActionResult Search(string keyword)
        {
            RefreshInfo();
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
            var listOfCategories = _categoriesService.GetCategories().Where(x => x.Disable == false);
            //pass to the view
            ViewBag.Categories = listOfCategories;
        }
    }
}
