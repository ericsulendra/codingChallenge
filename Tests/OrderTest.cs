namespace Tests
{
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
        }
    }
}
