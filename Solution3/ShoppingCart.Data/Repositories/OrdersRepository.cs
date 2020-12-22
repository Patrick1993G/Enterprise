using ShoppingCart.Data.Context;
using ShoppingCart.Domain.Interfaces;
using ShoppingCart.Domain.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace ShoppingCart.Data.Repositories
{
    public class OrdersRepository : IOrdersRepository
    {
        ShoppingCartDbContext _context;

        public OrdersRepository(ShoppingCartDbContext context)
        {
            _context = context;
        }
        public Guid AddOrder(Order o)
        {
            _context.Orders.Add(o);
            _context.SaveChanges();
            return o.Id;
        }

        public void DeleteOrder(Order o)
        {
            _context.Orders.Remove(o);
            _context.SaveChanges();
        }

        public Order GetOrder(Guid id)
        {
            return _context.Orders.SingleOrDefault(x => x.Id.Equals(id));
        }

        public IQueryable<Order> GetOrders()
        {
            return _context.Orders;
        }
    }
}
