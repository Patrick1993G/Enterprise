﻿using AutoMapper;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Newtonsoft.Json;
using ShoppingCart.Application.Interfaces;
using ShoppingCart.Application.ViewModels;
using ShoppingCart.Domain.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace PresentationWebApp.Controllers
{
    public class BasketController : Controller
    {
        const string SessionKeyName = "_Basket";
        private readonly IMapper _mapper;
        private readonly IProductsService _productsService;
        private readonly IOrdersService _ordersService;
        private readonly IOrderDetailsService _orderDetailsService;
        public BasketController(IProductsService productsService,IOrderDetailsService orderDetailsService, IOrdersService ordersService, IMapper mapper)
        {
            _productsService = productsService;
            _orderDetailsService = orderDetailsService;
            _ordersService = ordersService;
            _mapper = mapper;
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
            var productToAlter = products.FirstOrDefault(x => x.Id == product.Id);
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
        public IActionResult Checkout()
        {
            List<ProductViewModel> products = GetProducts();
            double total = 0;
            foreach (var item in products)
            {
                total += (item.Price*item.Quantity);
            }
            ViewBag.Total = total.ToString();
            return View("Checkout", products);
        }
        public IActionResult Pay()
        {
            List<ProductViewModel> products = GetProducts();
            DateTime datePlaced = DateTime.Now;
            string email = User.Identity.Name;
            //create order 
            OrderViewModel order = new OrderViewModel();
            order.DatePlaced = datePlaced;
            order.UserEmail = email;
            order.Id = _ordersService.AddOrder(order);
            foreach (var item in products)
            {    //decrease the stock
                _productsService.DecreaseStock(item.Id);
                OrderDetailsViewModel orderDetailsViewModel = new OrderDetailsViewModel();
                orderDetailsViewModel.Order = _mapper.Map<Order>(order);
                orderDetailsViewModel.Product = _mapper.Map<Product>(item);
                orderDetailsViewModel.Price = item.Price;
                orderDetailsViewModel.Quantity = item.Quantity;
                _orderDetailsService.AddOrderDetails(orderDetailsViewModel);
               
            }
             HttpContext.Session.SetString(SessionKeyName, "");
            TempData["feedback"] = ("Payment was successful");

            return RedirectToAction("Index");
        }
    }
}
