using ShoppingCart.Domain.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace ShoppingCart.Domain.Interfaces
{
    public interface ICategoriesRepository
    {
        
        IQueryable<Category> GetCategories();
        Category GetCategory(int id);
        void DeleteCategory(Category c);
        int AddCategory(Category c);
    }
}
