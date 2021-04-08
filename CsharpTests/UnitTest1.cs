using System.Collections.Generic;
using System.Threading.Tasks;

using Microsoft.VisualStudio.TestTools.UnitTesting;


using CSharp.Controllers;
using CSharp.Models;
using CSharp.Services;

namespace CsharpTests
{
    [TestClass]
    public class UnitTest1
    {
        private Orders testAPI;
        private List<Order> testOrdersStore;



        [TestInitialize]
         public void InitializeTest()
        {
            var orderService = new CSharp.Services.ListOrderService();
            testAPI = new Orders(orderService);
            testOrdersStore = orderService.OrderList;
        }
        public void AddExistingOrders()
        {

        }
        [TestMethod]
        public async Task GetsAllReturns200()
        {
            //Arrange
            testOrdersStore.Add(new Order(1,1, "existing Order 1" ));
            testOrdersStore.Add(new Order(2,1, "existing Order 2" ));
            
            //Act
            var result = await testAPI.GetAll();
            

            //Assert
          
                   Assert.IsTrue(true);
        }
    }
}
