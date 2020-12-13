using AutoMapper;
using ShoppingCart.Application.ViewModels;
using ShoppingCart.Domain.Models;
using System;
using System.Collections.Generic;
using System.Text;

namespace ShoppingCart.Application.AutoMapper
{
    class DomainToViewModelProfile:Profile
    {
        public DomainToViewModelProfile()
        {
            CreateMap<Profile, ProductViewModel>(); //ForMember(x=> x.Name, opt=> opt.MapFrom(src=>src.Name));

            CreateMap<Category, CategoryViewModel>();
            CreateMap<Member, MemberViewModel>();
        }
        

    }
}
