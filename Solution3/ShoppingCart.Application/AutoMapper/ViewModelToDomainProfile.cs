using AutoMapper;
using ShoppingCart.Application.ViewModels;
using ShoppingCart.Domain.Models;

namespace ShoppingCart.Application.AutoMapper
{
    public class ViewModelToDomainProfile:Profile
    {
        public ViewModelToDomainProfile()
        {
            CreateMap<ProductViewModel, Product>();
            CreateMap<CategoryViewModel,Category>();
            CreateMap<MemberViewModel, Member>();
        }
      
    }
}
