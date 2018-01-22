namespace CodingChallenge.Tests
{
    using System.Linq;
    using CodingChallenge.Models;
    using Microsoft.VisualStudio.TestTools.UnitTesting;
    using DataStore;

    [TestClass]
    public class Order4PacksTest
    {
        [TestMethod]
        public void Can_Fulfill_20_VS5_Order_test()
        {
            var dataStore = new TestDataStore();
            var order = new Order(dataStore);

            order.ProcessOrder("MB11", 62);
            var items = order.GetOrderSummary();
            var totalSize = items.Sum(i => i.Quantity * i.PackSize);
            Assert.IsTrue(totalSize == 62);
            Assert.AreEqual(0, order.UnfulfilledQuantity);
            Assert.IsTrue(order.IsOrderComplete);
        }
    }
}