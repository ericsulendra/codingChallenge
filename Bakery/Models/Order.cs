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
            // reset values
            orderItems.Clear();
            UnfulfilledQuantity = 0;
            IsOrderComplete = false;
            var qtyLeft = requestQty;
            var productCursor = 0;
            var packages = productStore.GetPackages();

            if(packages.Any(p => p.ProductCode == productCode))
            {
                // Largest pack first for backtracking later.
                var foundPackages = packages.Where(p => p.ProductCode == productCode).OrderByDescending(p => p.PackSize).ToList();
                
                // Check that requested quantity satisfies minimum quantity in pack
                // to be able to fulfill order
                if(requestQty < foundPackages.Min(p => p.PackSize))
                {
                    throw new OrderException(string.Format("Cannot fulfill order at this time. Minimum order is pack of {0}.", foundPackages.Min(p => p.PackSize)));
                }

                // initialize order items with 0 quantity for all possible packages to start
                foreach(var package in foundPackages)
                {
                    orderItems.Add(
                        new OrderItem () {
                            ProductCode = package.ProductCode,
                            PackPrice = package.UnitPrice,
                            PackSize = package.PackSize,
                            Quantity = 0
                        });
                }

                while(qtyLeft != 0)
                {
                    // Check for quick ones
                    // Since it is checking from largest pack first, it ensures least number of packs used
                    for (var index = productCursor; index < orderItems.Count; index++)                
                    {
                        var item = orderItems[index];
                        var remainder = qtyLeft % item.PackSize;
                        if(remainder == 0)
                        {
                            item.Quantity += qtyLeft / item.PackSize; 
                            qtyLeft -= qtyLeft;
                            break;                      
                        }
                        else 
                        {
                            item.Quantity += (qtyLeft - remainder) / item.PackSize;
                            qtyLeft = qtyLeft - (item.Quantity * item.PackSize);

                            // Remainder can't be fulfilled. Need to backtrack
                            if(item == orderItems.Last() && qtyLeft < item.PackSize)
                            {
                                // Tried smallest pack size and still can't fulfill order then it is impossible
                                // Need to break and record the remaining qty
                                if(productCursor == orderItems.Count - 1 &&
                                    orderItems.Take(orderItems.Count - 1).All(p => p.Quantity == 0))
                                {
                                    UnfulfilledQuantity = qtyLeft;
                                    qtyLeft = 0;
                                    break;
                                }    

                                // Remove the biggest filled pack size item first and try to fill it up again.
                                // set cursor forward to skip the larger pack size
                                var maxPackSizeItem = orderItems.LastOrDefault(p => p.Quantity != 0 && p != orderItems.Last());
                                if(maxPackSizeItem != null)
                                {
                                    maxPackSizeItem.Quantity -= 1;
                                    qtyLeft += maxPackSizeItem.PackSize;
                                    if(productCursor != orderItems.Count - 1)
                                    {                                        
                                        productCursor++;
                                    }
                                }
                                else 
                                {
                                    // smallest pack and unable to fulfill order.
                                    // terminate early
                                    UnfulfilledQuantity = qtyLeft;
                                    qtyLeft = 0;
                                    break;
                                }
                            }
                        }
                    }
                }

                if(UnfulfilledQuantity == 0)
                {
                    IsOrderComplete = true;
                }

                // Remove unneeded packs
                orderItems.RemoveAll(p => p.Quantity == 0);
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

        public decimal TotalPrice 
        {
            get 
            {
                return orderItems.Sum(i => i.TotalPrice);
            }
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