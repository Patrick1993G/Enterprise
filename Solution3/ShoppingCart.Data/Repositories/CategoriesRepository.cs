using ShoppingCart.Data.Context;
using ShoppingCart.Domain.Interfaces;
using ShoppingCart.Domain.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace ShoppingCart.Data.Repositories
{
    public class CategoriesRepository : ICategoriesRepository
    {
        ShoppingCartDbContext _context;

        public CategoriesRepository(ShoppingCartDbContext context)
        {
            _context = context;
        }

        public int AddCategory(Category c)
        {
            _context.Categories.Add(c);
            _context.SaveChanges();
            return c.Id;
        }

        public void DeleteCategory(Category c)
        {
            _context.Categories.Remove(c);
            _context.SaveChanges();
        }

        public void DisableCategory(int id)
        {
            var c = GetCategory(id);
            c.Disable = !c.Disable;
            _context.SaveChanges();
        }

        public IQueryable<Category> GetCategories()
        {
            return _context.Categories;
        }

        public Category GetCategory(int id)
        {
            return _context.Categories.SingleOrDefault(x => x.Id.Equals(id));
        }

    }
}
