using AutoMapper;
using ShoppingCart.Application.ViewModels;
using ShoppingCart.Domain.Models;

namespace ShoppingCart.Application.AutoMapper
{
    class DomainToViewModelProfile:Profile
    {
        public DomainToViewModelProfile()
        {
            CreateMap<Product, ProductViewModel>(); //ForMember(x=> x.Name, opt=> opt.MapFrom(src=>src.Name));
            CreateMap<Category, CategoryViewModel>();
            CreateMap<Member, MemberViewModel>();
            CreateMap<Order, OrderViewModel>();
            CreateMap<OrderDetails,OrderDetailsViewModel>();

        }


    }
}
