namespace Tests
{
    using System.Collections.Generic;
    using System.Linq;
    using CodingChallenge.Models;
    using Microsoft.VisualStudio.TestTools.UnitTesting;
    using Tests.DataStore;

    [TestClass]
    public class OrderTest
    {
        [TestMethod]
        public void Can_Fulfill_20_VS5_Order_test()
        {
            var dataStore = new TestDataStore();
            var order = new Order(dataStore);

            order.ProcessOrder("VS5", 20);
            var items = order.GetOrderSummary();
            var totalSize = items.Sum(i => i.Quantity * i.PackSize);
            Assert.IsTrue(totalSize == 20);
            Assert.AreEqual(0, order.UnfulfilledQuantity);
            Assert.IsTrue(order.IsOrderComplete);
        }

        [TestMethod]
        public void Cannot_Fulfill_4_VS5_Order_test()
        {
             var dataStore = new TestDataStore();
            var order = new Order(dataStore);

            order.ProcessOrder("VS5", 4);
            var items = order.GetOrderSummary();
            var totalSize = items.Sum(i => i.Quantity * i.PackSize);
            Assert.IsTrue(totalSize == 3);
        }
        
        [TestMethod]
        public void Cannot_Fulfill_7_VS5_Order_test()
        {
             var dataStore = new TestDataStore();
            var order = new Order(dataStore);

            order.ProcessOrder("VS5", 7);
            var items = order.GetOrderSummary();
            var totalSize = items.Sum(i => i.Quantity * i.PackSize);
            Assert.IsTrue(totalSize == 6);
            Assert.AreEqual(1, order.UnfulfilledQuantity);
            Assert.IsFalse(order.IsOrderComplete);
        }

        [TestMethod]
        public void Can_Fulfill_12_VS5_Order_test()
        {
            var dataStore = new TestDataStore();
            var order = new Order(dataStore);

            var packages = dataStore.GetPackages();
            var qty = 14;
            var code = "VS5";

            var orderPackages = packages.Where(p => p.ProductCode == code).OrderByDescending(p => p.PackSize).ToList();

            // Need to keep track of state
            // initialize order items with 0 quantity for all possible packages
            var tempOrderItems = new List<OrderItem>();
            foreach(var package in orderPackages)
            {
                tempOrderItems.Add(
                    new OrderItem () {
                        ProductCode = package.ProductCode,
                        PackPrice = package.UnitPrice,
                        PackSize = package.PackSize,
                        Quantity = 0
                    });
            }


            //order.ProcessOrder("VS5", 12);
            // var items = order.GetOrderSummary();
            // var totalSize = items.Sum(i => i.Quantity * i.PackSize);
            // Assert.IsTrue(totalSize == 12);
        }
    }
}
