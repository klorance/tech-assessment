using System.Collections.Generic;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using System.Linq;

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
            testOrdersStore.Clear();
        }
        public void AddExistingOrders()
        {

        }
        [TestMethod]
        public async Task GetsAllReturns200AndResults()
        {
            //Arrange
            testOrdersStore.Add(new Order(1,1, "existing Order 1" ));
            testOrdersStore.Add(new Order(2,1, "existing Order 2" ));
            
            //Act
            var result = await testAPI.GetAll() as OkObjectResult;
            Assert.AreEqual(result.StatusCode,200);
            Assert.IsInstanceOfType(result.Value, typeof(IEnumerable<Order>));
            IEnumerable<Order> orders =  result.Value as IEnumerable<Order>;
            Assert.IsTrue(orders.Count() == 2 );
        }

        [TestMethod]
        public async Task GetsAllDoesNotReturnCancelledOrders()
        {
            //Arrange
            testOrdersStore.Add(new Order(1,1, "existing Order 1" ));
            testOrdersStore.Add(new Order(2,1, "existing Order 2" ));
            Order cancelledOrder = new Order(3,1, "existing Order Cancelled" );
            cancelledOrder.Cancelled = true;
            testOrdersStore.Add(cancelledOrder);
            
            //Act
            var result = await testAPI.GetAll() as OkObjectResult;
            Assert.AreEqual(result.StatusCode,200);
            Assert.IsInstanceOfType(result.Value, typeof(IEnumerable<Order>));
            IEnumerable<Order> orders =  result.Value as IEnumerable<Order>;
            Assert.IsTrue(orders.Count() == 2 );
            Assert.IsFalse(orders.Any(o => o.Cancelled));
        }

        [TestMethod]
        public async Task PostItemReturnsCreatedAndItemCanBeFoundByUpdatedID()
        {
            var result = await testAPI.Create(new Order(0,1,"CreatedByTest")) as CreatedAtActionResult;
            Assert.IsNotNull(result);
            Assert.IsTrue((result.Value as Order).Id == 1); //based on logic for first item added to empty list
            var verifyResult = await testAPI.GetByOrderID(1) as OkObjectResult; 
            Order verifyNewOrder = verifyResult.Value as Order;
            Assert.AreEqual(verifyNewOrder.Description, "CreatedByTest");
        }

        [TestMethod]
        public async Task GetNonExistingItemReturns404()
        {
            var verifyResult = await testAPI.GetByOrderID(1) as NotFoundResult; //test init clears existing items
            Assert.IsNotNull(verifyResult);
        }
        
         [TestMethod]
        public async Task validateReturnTypesForDeleting()
        {
            testOrdersStore.Add(new Order(1,1, "existing Order 1" ));
            var result = await testAPI.Delete(1);
            Assert.IsNotNull(result);
            Assert.IsNotInstanceOfType(result, typeof(NotFoundResult));
            var secondResult = await testAPI.Delete(1) as NotFoundResult;
            Assert.IsNotNull(secondResult);

        }

         [TestMethod]
        public async Task validateReturnOfFoundCustomerItems()
        {
            testOrdersStore.Add(new Order(1,1, "existing Order 1 customer 1" ));
            testOrdersStore.Add(new Order(2,1, "existing Order 2 customer 1" ));
            testOrdersStore.Add(new Order(3,1, "existing Order 3 customer 1" ));
            testOrdersStore.Add(new Order(4,1, "existing Order 4 customer 2" ));
            testOrdersStore.Add(new Order(5,2, "existing Order 5 customer 2" ));
            testOrdersStore.Add(new Order(6,2, "existing Order 6 customer 2" ));
            testOrdersStore.Add(new Order(7,2, "existing Order 7 customer 2" ));
            var result = await testAPI.Delete(1);
            Assert.IsNotNull(result);
            Assert.IsNotInstanceOfType(result, typeof(NotFoundResult));
            var secondResult = await testAPI.Delete(1) as NotFoundResult;
            Assert.IsNotNull(secondResult);

        }
         [TestMethod]
        public async Task validateNotFoundForMissingCustomer()
        {
            testOrdersStore.Add(new Order(1,1, "existing Order 1" ));
            var result = await testAPI.Delete(1);
            Assert.IsNotNull(result);
            Assert.IsNotInstanceOfType(result, typeof(NotFoundResult));
            var secondResult = await testAPI.Delete(1) as NotFoundResult;
            Assert.IsNotNull(secondResult);

        }

        //NOTE at this point I feel additional functional testing of the controller would be exercising the repository logic more than the controller behavior
        //Additional testing to validate verb usage and routing expectations would also be warranted but is easier to confirm with integration testing


    }
}
