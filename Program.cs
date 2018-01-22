namespace CodingChallenge
{
    using System;
    using System.Collections.Generic;
    using System.IO;
    using System.Linq;
    using Models;

    class Package : Product
    {
        public int Quantity { get;set; }        
    }

    class OrderItem
    {
        public int Quantity { get;set; }
        public string ProductCode { get;set; }
        public decimal TotalPrice { get;set; }

        public override string ToString()
        {
            return string.Format("{0} {1} {2}", Quantity, ProductCode, TotalPrice);
        }
    }

    class Order
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

    class Initialize
    {
        public void ReadProductFile(string path)
        {

        }
    }

    public class Program
    { 
        static void Main(string[] args)
        {

            var products = new List<Package>();
            products.Add(new Package{
                ProductCode = "VS5",
                ProductName = "Vegemite Scroll",
                UnitPrice = 6.99m,
                Quantity = 3                
            });
            products.Add(new Package{
                ProductCode = "VS5",
                ProductName = "Vegemite Scroll",
                UnitPrice = 8.99m,
                Quantity = 5                
            });

            products.Add(new Package{
                ProductCode = "MB11",
                ProductName = "Blueberry Muffin",
                UnitPrice = 9.95m,
                Quantity = 2
            });
            products.Add(new Package{
                ProductCode = "MB11",
                ProductName = "Blueberry Muffin",
                UnitPrice = 16.95m,
                Quantity = 5
            });
            products.Add(new Package{
                ProductCode = "MB11",
                ProductName = "Blueberry Muffin",
                UnitPrice = 24.95m,
                Quantity = 8
            });

            products.Add(new Package{
                ProductCode = "CF",
                ProductName = "Croissant",
                UnitPrice = 5.95m,
                Quantity = 3
            });
            products.Add(new Package{
                ProductCode = "CF",
                ProductName = "Croissant",
                UnitPrice = 9.95m,
                Quantity = 5
            });
             products.Add(new Package{
                ProductCode = "CF",
                ProductName = "Croissant",
                UnitPrice = 13.99m,
                Quantity = 7
            });
            products.Add(new Package{
                ProductCode = "CF",
                ProductName = "Croissant",
                UnitPrice = 16.99m,
                Quantity = 9
            });

            if(!args.Any())
            {
                Console.WriteLine("please specify input data");
                Console.ReadKey();
            }
            else
            {
                try
                {
                    using(var inputFile = File.OpenText(args[0]))
                    {
                        string line;
                    
                        while((line = inputFile.ReadLine()) != null)
                        {
                            if(!line.StartsWith("#") && !string.IsNullOrEmpty(line)) 
                            {
                                Console.WriteLine(line);
                                var split = line.Split(new[] {' '}, StringSplitOptions.RemoveEmptyEntries);
                                
                                // Only process text that has at least two phrases
                                // Purposely used larger than 2 and ignore other extra text
                                if(split.Any() && split.Length >= 2)
                                {
                                    var order = new Order();
                                    var requestQty = int.Parse(split[0]);
                                    var code = split[1];

                                    if(products.Any(p => p.ProductCode == code))
                                    {
                                        var foundProducts = products.Where(p => p.ProductCode == code).OrderBy(p => p.Quantity).ToList();
                                        
                                        // Check that requested quantity satisfies minimum quantity in pack
                                        // to be able to fulfill order
                                        if(requestQty < foundProducts.Min(p => p.Quantity))
                                        {
                                            Console.WriteLine("Cannot fulfill order at this time. Minimum order is {0} items", foundProducts.Min(p => p.Quantity));
                                            continue;
                                        }

                                        var maxPackSize = foundProducts.Max(p => p.Quantity);
                                        var minPackSize = foundProducts.Min(p => p.Quantity);

                                        while(requestQty > 0)
                                        {  
                                            int passIndex = foundProducts.Count - 1;
                                            int productIndex = foundProducts.Count - 1;
                                            if(requestQty < minPackSize)
                                            {
                                                order.FulfillOrder(1, requestQty, code, 2.5m);
                                                break;
                                            }
                                            while (productIndex >= 0)                                      
                                            {
                                                var product = foundProducts[productIndex];

                                                if(product.Quantity > requestQty)
                                                {
                                                    productIndex--;
                                                    continue;
                                                }

                                                var remainder = requestQty % product.Quantity;
                                                
                                                if(remainder == 0)
                                                {
                                                    order.FulfillOrder(requestQty / product.Quantity, product.Quantity, product.ProductCode, product.UnitPrice);
                                                    requestQty = 0;
                                                    break;
                                                }
                                                else
                                                {
                                                    var packCount = (int)Math.Floor((decimal)requestQty / product.Quantity);

                                                    order.FulfillOrder(packCount, product.Quantity, product.ProductCode, product.UnitPrice);
                                                    requestQty -= product.Quantity * packCount;
                                                    productIndex--;
                                                }
                                            }
                                        }

                                        // print fullfilled order                                        
                                        var items = order.GetPackedItems();
                                        items.ForEach(item => Console.WriteLine(item));
                                    }
                                    else
                                    {
                                        Console.WriteLine("Cannot find Product Code: {0}, please verify again.", code);
                                    }
                                }
                            }
                        }
                    }
                }
                catch(Exception ex)
                {                    
                    Console.WriteLine("Error has occured. Please try again");
                    Console.WriteLine(ex);                
                }
            }
        }
    }
}
