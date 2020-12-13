using AutoMapper;
using ShoppingCart.Application.Interfaces;
using ShoppingCart.Application.ViewModels;
using ShoppingCart.Domain.Interfaces;
using ShoppingCart.Domain.Models;
using System;
using System.Collections.Generic;
using System.Text;

namespace ShoppingCart.Application.Services
{
    public class MembersService : IMembersService
    {
        private IMapper _mapper;
        private IMembersRepository _membersRepo;
        public MembersService(IMembersRepository membersRepository, IMapper mapper)
        {
            _mapper = mapper;
            _membersRepo = membersRepository;
        }

        public void AddMember(MemberViewModel m)
        {
            Member member = _mapper.Map<MemberViewModel, Member>(m);
            _membersRepo.AddMember(member);
        }
    }
}
