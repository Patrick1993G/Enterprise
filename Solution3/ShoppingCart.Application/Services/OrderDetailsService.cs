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
    public class OrderDetailsService : IOrderDetailsService
    {
        private IMapper _mapper;
        private IOrderDetailsRepository _orderDetailsRepository;
        public OrderDetailsService(IOrderDetailsRepository orderDetailsRepository, IMapper mapper)
        {
            _mapper = mapper;
            _orderDetailsRepository = orderDetailsRepository;
        }
        public void AddOrderDetails(OrderDetailsViewModel orderDetails)
        {
            _orderDetailsRepository.AddOrderDetails(_mapper.Map<OrderDetails>(orderDetails));
        }
        public int DeleteOrderDetails(int id)
        {
            var oToDelete = _orderDetailsRepository.GetOrderDetails(id);
            if (oToDelete != null)
            {
                _orderDetailsRepository.DeleteOrderDetails(oToDelete);
            }
            return oToDelete.Id;
        }

        public IQueryable<OrderDetailsViewModel> GetOrderDetails()
        {
            var myOrder = _orderDetailsRepository.GetOrderDetails().ProjectTo<OrderDetailsViewModel>(_mapper.ConfigurationProvider);
            return myOrder;
        }

        public OrderDetailsViewModel GetOrderDetails(int id)
        {
            var myOrder = _orderDetailsRepository.GetOrderDetails(id);
            var myModel = _mapper.Map<OrderDetailsViewModel>(myOrder);
            return myModel;
        }
    }
}
