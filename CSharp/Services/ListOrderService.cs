using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using CSharp.Models;
using CSharp.Services.Interfaces;

namespace CSharp.Services
{
    public class ListOrderService : IOrderService
    {
        // this class uses a simple list as a backing store for this exercise purpos only
        // ideally I would have another abstraction layer with dependency injection to handle the repository work
        public ListOrderService()
        {
            OrderList = new List<Order>();
            OrderList.Add(new Order(1,1,"existing order"));
        }

        public Task<Order> Create(Order order)
        {
            int newOrderId = OrderList.Count > 0 ? OrderList.Max(o => o.Id) + 1 : 1 ;
            order.Id = newOrderId;
            OrderList.Add(order);
            return Task<Order>.FromResult(order);
        }

        public Task<bool> Delete(int orderId)
        {
            //soft delete by setting cancelled property
            bool found = OrderList.Any(o => (o.Id == orderId) && (!o.Cancelled));
            if (found)
            {
                OrderList.First(o => o.Id == orderId).Cancelled = true;
            }
            return Task<bool>.FromResult(found);
        }

        public Task<Order> Get(int orderId)
        {
            return Task<Order>.FromResult( OrderList.FirstOrDefault(o => (o.Id == orderId) && (!o.Cancelled)));
        }

        public Task<IEnumerable<Order>> GetAll()
        {
            return Task<Order>.FromResult( OrderList.Where(o => !o.Cancelled));
        }

        public Task<IEnumerable<Order>> GetAllByCustomerId(int customerId)
        {
            return Task<Order>.FromResult( OrderList.Where(o => (o.CustomerId == customerId) && (!o.Cancelled)));
        }

        public Task<Order> Update(Order order)
        {
            //TODO not very pretty
            OrderList[OrderList.FindIndex(o => o.Id == order.Id)] = order;
            return Task<Order>.FromResult(order);
        }

        //Made this public to facilitate unit testing
        public List<Order> OrderList { get; set; }
    }
}