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
    public class OrdersService : IOrdersService
    {
        private IMapper _mapper;
        private IOrdersRepository _ordersRepository;
        public OrdersService(IOrdersRepository ordersRepository, IMapper mapper)
        {
            _mapper = mapper;
            _ordersRepository = ordersRepository;
        }

        public void AddOrder(OrderViewModel order)
        {
            _ordersRepository.AddOrder(_mapper.Map<Order>(order));
        }

        public Guid DeleteOrder(Guid id)
        {
            var oToDelete = _ordersRepository.GetOrder(id);
            if (oToDelete != null)
            {
                _ordersRepository.DeleteOrder(oToDelete);
            }
            return oToDelete.Id;
        }

        public IQueryable<OrderViewModel> GetOrders()
        {
            var myOrder = _ordersRepository.GetOrders().ProjectTo<OrderViewModel>(_mapper.ConfigurationProvider);
            return myOrder;
        }

        public OrderViewModel GetOrder(Guid id)
        {
            var myOrder = _ordersRepository.GetOrder(id);
            var myModel = _mapper.Map<OrderViewModel>(myOrder);
            return myModel;
        }
    }
}
