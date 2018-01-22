namespace CodingChallenge.Models
{
    using System.Collections.Generic;
    using System.Linq;

    public class Order
    {
        private List<OrderItem> orderItems { get;set; }
        public Order()
        {
            orderItems = new List<OrderItem>();
        }

        public void FulfillOrder(int quantity, int packSize, string productCode, decimal packPrice)
        {
            for(var c = 0; c < quantity; c++)
            {
                orderItems.Add(new OrderItem{
                    ProductCode = productCode,
                    Quantity = packSize,
                    TotalPrice = packPrice
                });
            }
        }
        
        public void RemoveItem(int quantity, int packSize, string productCode)
        {
            for(var c = 0; c < quantity; c++)
            {
                var itemToRemove = orderItems.Find(item => item.ProductCode == productCode && item.Quantity == packSize);
                if(itemToRemove != null)
                {
                    orderItems.Remove(itemToRemove);
                }
            }
        }

        public List<OrderItem> GetPackedItems()
        {
            var items = (from i in orderItems
                        group i by i.Quantity                        
                        into g
                        select new OrderItem
                        {
                            Quantity = g.Sum(p => p.Quantity),
                            TotalPrice = g.Sum(p => p.TotalPrice),
                            ProductCode = g.First().ProductCode                            
                        }).ToList();
            return items;
        }
    }
}