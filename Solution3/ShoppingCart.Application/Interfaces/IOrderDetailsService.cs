using ShoppingCart.Application.ViewModels;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace ShoppingCart.Application.Interfaces
{
    public interface IOrderDetailsService
    {
        IQueryable<OrderDetailsViewModel> GetOrderDetails();
        OrderDetailsViewModel GetOrderDetails(int id);
        void AddOrderDetails(OrderDetailsViewModel OrderDetails);
        int DeleteOrderDetails(int id);
    }
}
