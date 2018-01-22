namespace CodingChallenge
{
    using System;
    using System.Collections.Generic;
    using System.IO;
    using System.Linq;
    using CodingChallenge.Models;
    using CodingChallenge.Data;

    public class Bakery
    {       
        public static void Main(string[] args)
        {
            if(!args.Any() || args.Length != 2)
            {
                Console.WriteLine("please specify input data with <order-file> <product-file> arguments");
                Console.ReadKey();
            }
            else
            {
                try
                {                    
                    var productStore = new FileProductStore(args[1]);
                    var products = new List<Package>();
                    products = productStore.GetPackages();

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
