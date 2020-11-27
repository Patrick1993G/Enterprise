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
    public class CategoriesService : ICategoriesService
    {
        private ICategoriesRepository _categoriesRepo;
        public CategoriesService(ICategoriesRepository categoriesRepository)
        {
            _categoriesRepo = categoriesRepository;
        }

        public void AddCategory(CategoryViewModel category)
        {
            Category c = new Category()
            {
                Name = category.Name
            };
            _categoriesRepo.AddCategory(c);
        }

        public IQueryable<CategoryViewModel> GetCategories()
        {
            var list = from c in _categoriesRepo.GetCategories()
                       select new CategoryViewModel
                       {
                           Id = c.Id,
                           Name = c.Name
                       };
            return list;
        }

        public CategoryViewModel GetCategory(int id)
        {
            var myCategory = _categoriesRepo.GetCategory(id);
            CategoryViewModel myModel = new CategoryViewModel();
            myModel.Name = myCategory.Name;
            return myModel;
        }
    }
}
