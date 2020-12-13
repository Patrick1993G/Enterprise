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
    public class CategoriesService : ICategoriesService
    {
        private IMapper _mapper;
        private ICategoriesRepository _categoriesRepo;
        public CategoriesService(ICategoriesRepository categoriesRepository, IMapper mapper)
        {
            _mapper = mapper;
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
            var categories = _categoriesRepo.GetCategories();
            var list = _mapper.Map<IQueryable<Category>, IQueryable<CategoryViewModel>>(categories);
            return list;
        }

        public CategoryViewModel GetCategory(int id)
        {
            var myCategory = _categoriesRepo.GetCategory(id);
            CategoryViewModel myModel = _mapper.Map<Category,CategoryViewModel>(myCategory);
            return myModel;
        }
    }
}
