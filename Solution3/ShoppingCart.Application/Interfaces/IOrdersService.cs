﻿using ShoppingCart.Application.ViewModels;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace ShoppingCart.Application.Interfaces
{
    public interface IOrdersService
    {
        IQueryable<OrderViewModel> GetOrders();
        OrderViewModel GetOrder(Guid id);
        Guid AddOrder(OrderViewModel Order);
        Guid DeleteOrder(Guid id);
    }
}
