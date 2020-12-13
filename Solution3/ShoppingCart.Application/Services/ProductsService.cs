using AutoMapper;
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
        private IMapper _mapper;
        private IProductsRepository _productsRepo;
        public ProductsService(IProductsRepository productsRepository, IMapper mapper)
        {
            _mapper = mapper;
            _productsRepo = productsRepository;
        }

        public void AddProduct(ProductViewModel myProduct)
        {
           
            Product product = _mapper.Map<ProductViewModel,Product>(myProduct);
            _productsRepo.AddProduct(product);
        }

        public ProductViewModel GetProduct(Guid id)
        {
            var myProduct = _productsRepo.GetProduct(id);
            ProductViewModel myModel = _mapper.Map< Product,ProductViewModel>(myProduct);
            return myModel;
        }

        public IQueryable<ProductViewModel> GetProducts()
        {
            var products = _productsRepo.GetProducts();
            var result = _mapper.Map<IQueryable<Product>, IQueryable<ProductViewModel>>(products);
            return result;
        }

        public IQueryable<ProductViewModel> GetProducts(int category)
        {
            var list = _productsRepo.GetProducts().Where(x => x.Category.Id == category);
            var result = _mapper.Map<IQueryable<Product>, IQueryable<ProductViewModel>>(list);
            return result;
        }

        public Guid DeleteProduct(Guid id)
        {
            var pToDelete = _productsRepo.GetProduct(id);
            if (pToDelete != null)
            {
                _productsRepo.DeleteProduct(pToDelete);
            }
            return pToDelete.Id;
        }

        public Guid DisableProduct(Guid id)
        {

            if (id != null)
            {
                _productsRepo.DisableProduct(id);
            }
            return id;
        }
    }
}
