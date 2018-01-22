namespace CodingChallenge.Tests
{
    using System.Collections.Generic;
    using System.Linq;
    using CodingChallenge.Models;
    using Microsoft.VisualStudio.TestTools.UnitTesting;
    using CodingChallenge.Tests.DataStore;

    [TestClass]
    public class Order2PacksTest
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
            Assert.AreEqual(35.96m, order.TotalPrice);
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
            Assert.AreEqual(6.99m, order.TotalPrice);
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
            Assert.AreEqual(13.98m, order.TotalPrice);
        }

        [TestMethod]
        public void Can_Fulfill_12_VS5_Order_test()
        {
            var dataStore = new TestDataStore();
            var order = new Order(dataStore);
            order.ProcessOrder("VS5", 12);
            var items = order.GetOrderSummary();
            var totalSize = items.Sum(i => i.Quantity * i.PackSize);
            Assert.IsTrue(totalSize == 12);
            Assert.AreEqual(27.96m, order.TotalPrice);
        }
    }
}
