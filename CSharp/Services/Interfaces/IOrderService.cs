using System.Linq;
using System.Threading.Tasks;
using CSharp.Models;
using System.Collections.Generic;

namespace CSharp.Services.Interfaces
{
    public interface IOrderService
    {
       Task<Order> Create(Order order);

        Task<Order> Update(Order order);

        Task<Order> Get(int orderId);

        Task<IEnumerable<Order>> GetAll();

        Task<IEnumerable<Order>> GetAllByCustomerId(int customerId);

        Task<bool> Delete(int orderId);
         
    }
}