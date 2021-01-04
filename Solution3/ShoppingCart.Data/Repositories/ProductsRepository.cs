using ShoppingCart.Data.Context;
using ShoppingCart.Domain.Interfaces;
using ShoppingCart.Domain.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Security.Cryptography.X509Certificates;
using System.Text;

namespace ShoppingCart.Data.Repositories
{
    public class ProductsRepository : IProductsRepository
    {
        ShoppingCartDbContext _context;
        public ProductsRepository(ShoppingCartDbContext context)
        {
            _context = context;
        }
        public Guid AddProduct(Product p)
        {
            _context.Products.Add(p);
            _context.SaveChanges();
            return p.Id;
        }

        public int DecreaseStock(Guid id)
        {
            var p = GetProduct(id);
            p.Stock--;
            _context.SaveChanges();
            return p.Stock;
        }
        public int IncreaseStock(Guid id)
        {
            var p = GetProduct(id);
            p.Stock++;
            _context.SaveChanges();
            return p.Stock;
        }
        public void DeleteProduct(Product p)
        {
            _context.Products.Remove(p);
            _context.SaveChanges();
        }
        public void DisableProduct(Guid id)
        {
            var p = GetProduct(id);
            p.Disable = !p.Disable;
            _context.SaveChanges();
        }

        public Product GetProduct(Guid id)
        {
            return _context.Products.SingleOrDefault(x => x.Id.Equals(id));
        }

        public IQueryable<Product> GetProducts()
        {
            return _context.Products;
        }
    }
}
