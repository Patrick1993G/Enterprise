using ShoppingCart.Data.Context;
using ShoppingCart.Domain.Interfaces;
using ShoppingCart.Domain.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace ShoppingCart.Data.Repositories
{
    public class OrderDetailsRepository : IOrderDetailsRepository
    {

        ShoppingCartDbContext _context;

        public OrderDetailsRepository(ShoppingCartDbContext context)
        {
            _context = context;
        }

        public int AddOrderDetails(OrderDetails o)
        {
            _context.OrderDetails.Add(o);
            _context.SaveChanges();
            return o.Id;
        }

        public void DeleteOrderDetails(OrderDetails o)
        {
            _context.OrderDetails.Remove(o);
            _context.SaveChanges();
        }

        public IQueryable<OrderDetails> GetOrderDetails()
        {
            return _context.OrderDetails;
        }

        public OrderDetails GetOrderDetails(int id)
        {
            return _context.OrderDetails.SingleOrDefault(x => x.Id.Equals(id));
        }
    }
}
