using ShoppingCart.Application.Interfaces;
using ShoppingCart.Application.ViewModels;
using ShoppingCart.Domain.Interfaces;
using ShoppingCart.Domain.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace ShoppingCart.Application.Services
{

    public class ProductsService : IProductsService
    {
        private IProductsRepository _productsRepo;
        public ProductsService(IProductsRepository productsRepository)
        {
            _productsRepo = productsRepository;
        }

        public void AddProduct(ProductViewModel myProduct)
        {
            Product product = new Product()
            {
                Description = myProduct.Description,
                CategoryId = myProduct.Category.Id,
                ImageUrl = myProduct.ImageUrl,
                Name = myProduct.Name,
                Price = myProduct.Price
            };
            _productsRepo.AddProduct(product);
        }

        public ProductViewModel GetProduct(Guid id)
        {
            var myProduct = _productsRepo.GetProduct(id);
            ProductViewModel myModel = new ProductViewModel();
            myModel.Category = new CategoryViewModel()
            {
                Id = myProduct.Category.Id,
                Name = myProduct.Category.Name
            };
            myModel.Description = myProduct.Description;
            myModel.Id = myProduct.Id;
            myModel.ImageUrl = myProduct.ImageUrl;
            myModel.Name = myProduct.Name;
            myModel.Price = myProduct.Price;
            return myModel;
        }

        public IQueryable<ProductViewModel> GetProducts()
        {

            var list = from p in _productsRepo.GetProducts()
                       select new ProductViewModel()
                       {
                           Id = p.Id,
                           Description = p.Description,
                           Name = p.Name,
                           Price = p.Price,
                           Category = new CategoryViewModel() { Id = p.Category.Id, Name = p.Category.Name},
                           ImageUrl = p.ImageUrl
                        };
            return list;
        }

        public IQueryable<ProductViewModel> GetProducts(int category)
        {
            var list = from p in _productsRepo.GetProducts().Where(x => x.Category.Id == category)
                       select new ProductViewModel()
                       {
                           Id = p.Id,
                           Description = p.Description,
                           Name = p.Name,
                           Price = p.Price,
                           Category = new CategoryViewModel() { Id = p.Category.Id, Name = p.Category.Name },
                           ImageUrl = p.ImageUrl
                       };
            return list;
        }

        public Guid DeleteProduct(ProductViewModel myProduct)
        {
            Product product = new Product()
            {
                Description = myProduct.Description,
                CategoryId = myProduct.Category.Id,
                ImageUrl = myProduct.ImageUrl,
                Name = myProduct.Name,
                Price = myProduct.Price,
                Id = myProduct.Id
            };
            _productsRepo.DeleteProduct(product);
            return myProduct.Id;
        }
    }
}
