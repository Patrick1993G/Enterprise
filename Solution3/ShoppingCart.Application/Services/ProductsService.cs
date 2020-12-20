using AutoMapper;
using AutoMapper.QueryableExtensions;
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
            var product = _mapper.Map<Product>(myProduct);
            product.Category = null;
            _productsRepo.AddProduct(product);
            
        }

        public ProductViewModel GetProduct(Guid id)
        {
            var myProduct = _productsRepo.GetProduct(id);
            var myModel = _mapper.Map<ProductViewModel>(myProduct);
            return myModel;
        }

        public IQueryable<ProductViewModel> GetProducts()
        {
            var products = _productsRepo.GetProducts().ProjectTo<ProductViewModel>(_mapper.ConfigurationProvider);
            return products;
        }

        public IQueryable<ProductViewModel> GetProducts(int category)
        {
            var list = _productsRepo.GetProducts().Where(x => x.Category.Id == category)
                .ProjectTo<ProductViewModel>(_mapper.ConfigurationProvider);
            return list;
        }
        public IQueryable<ProductViewModel> GetProducts(string keyword)
        {
            var result = _productsRepo.GetProducts().Where(x => x.Description.Contains(keyword) || x.Name.Contains(keyword))
                .ProjectTo<ProductViewModel>(_mapper.ConfigurationProvider);
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
