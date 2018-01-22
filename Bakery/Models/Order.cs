namespace CodingChallenge.Models
{
    using System;
    using System.Collections.Generic;
    using System.Linq;
    using CodingChallenge.Data;
    using CodingChallenge.Exceptions;

    public class Order
    {
        private List<OrderItem> orderItems { get;set; }
        private IProductStore productStore { get;set; }
        public bool IsOrderComplete { get;set; }
        public int UnfulfilledQuantity { get; set; } 
        public Order(IProductStore productStore)
        {
            orderItems = new List<OrderItem>();
            this.productStore = productStore;
        }

        // Process a new order with a given product code and request quantity
        public void ProcessOrder(string productCode, int requestQty)
        {
            orderItems.Clear();
            var packages = productStore.GetPackages();
            if(packages.Any(p => p.ProductCode == productCode))
            {
                var foundProducts = packages.Where(p => p.ProductCode == productCode).OrderBy(p => p.PackSize).ToList();
                
                // Check that requested quantity satisfies minimum quantity in pack
                // to be able to fulfill order
                if(requestQty < foundProducts.Min(p => p.PackSize))
                {
                    throw new OrderException(string.Format("Cannot fulfill order at this time. Minimum order is pack of {0}.", foundProducts.Min(p => p.PackSize)));
                }

                var maxPackSize = foundProducts.Max(p => p.PackSize);
                var minPackSize = foundProducts.Min(p => p.PackSize);

                while(requestQty > 0)
                {  
                    int passIndex = foundProducts.Count - 1;
                    int productIndex = foundProducts.Count - 1;
                    if(requestQty < minPackSize)
                    {
                        UnfulfilledQuantity = requestQty;
                        IsOrderComplete = false;
                        break;
                    }
                    while (productIndex >= 0)                                      
                    {
                        var product = foundProducts[productIndex];

                        if(product.PackSize > requestQty)
                        {
                            productIndex--;
                            continue;
                        }

                        var remainder = requestQty % product.PackSize;
                        
                        if(remainder == 0)
                        {
                            FulfillOrder(requestQty / product.PackSize, product.PackSize, product.ProductCode, product.UnitPrice);
                            requestQty = 0;
                            break;
                        }
                        else
                        {
                            var packCount = (int)Math.Floor((decimal)requestQty / product.PackSize);

                            FulfillOrder(packCount, product.PackSize, product.ProductCode, product.UnitPrice);
                            requestQty -= product.PackSize * packCount;
                            productIndex--;
                        }
                    }
                }    
            }
            else
            {
                throw new OrderException(string.Format("No product with this code {0}, please verify again", productCode));
            }
        }

        // Return order summary grouped by pack size
        public List<OrderItem> GetOrderSummary()
        {
            return orderItems;
        }
        
        public void FulfillOrder(int quantity, int packSize, string productCode, decimal packPrice)
        {
            var existingItem = orderItems.Find(item => item.ProductCode == productCode && item.PackSize == packSize);
            if(existingItem != null)
            {
                existingItem.Quantity += quantity;
            }
            else
            {
                orderItems.Add(new OrderItem{
                    ProductCode = productCode,
                    Quantity = quantity,
                    PackSize = packSize,
                    PackPrice = packPrice
                });
            }
        }
        
        public void RemoveItem(int quantityToRemove, int packSize, string productCode)
        {
            for(var c = 0; c < quantityToRemove; c++)
            {
                var itemToRemove = orderItems.Find(item => item.ProductCode == productCode && item.PackSize == packSize);
                if(itemToRemove != null)
                {
                    if(quantityToRemove >= itemToRemove.Quantity)
                    {
                        orderItems.Remove(itemToRemove);
                    }
                    else
                    {
                        itemToRemove.Quantity -= quantityToRemove;
                    }
                }
            }
        }
    }
}