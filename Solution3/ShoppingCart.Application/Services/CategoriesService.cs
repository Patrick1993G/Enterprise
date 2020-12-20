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
            _categoriesRepo.AddCategory(_mapper.Map<Category>(category));
        }

        public int DeleteCategory(int id)
        {
            var cToDelete = _categoriesRepo.GetCategory(id);
            if (cToDelete != null)
            {
                _categoriesRepo.DeleteCategory(cToDelete);
            }
            return cToDelete.Id;
        }
        public int DisableCategory(int id)
        {

            if (id > 0)
            {
                _categoriesRepo.DisableCategory(id);
            }
            return id;
        }
        public IQueryable<CategoryViewModel> GetCategories()
        {
            var categories = _categoriesRepo.GetCategories().ProjectTo<CategoryViewModel>(_mapper.ConfigurationProvider);
            return categories;
        }

        public CategoryViewModel GetCategory(int id)
        {
            var myCategory = _categoriesRepo.GetCategory(id);
            var myModel = _mapper.Map<CategoryViewModel>(myCategory);
            return myModel;
        }

    }
}
