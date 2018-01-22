namespace CodingChallenge
{
    using System;
    using System.Collections.Generic;
    using System.IO;
    using System.Linq;
    using CodingChallenge.Models;
    using CodingChallenge.Data;
    using CodingChallenge.Exceptions;

    public class Bakery
    {
        public static void Main(string[] args)
        {
            if(!args.Any() || args.Length != 2)
            {
                Console.WriteLine("please specify input data with <order-file-location> <product-file-location> arguments");
                Console.ReadKey();
            }
            else
            {
                try
                {                    
                    var productStore = new FileProductStore(args[1]);
                    var products = new List<Package>();

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
                                    var requestQty = int.Parse(split[0]);
                                    var code = split[1];
                                    
                                    var order = new Order(productStore);

                                    try
                                    {                                        
                                        order.ProcessOrder(code, requestQty);
                                    }
                                    catch (OrderException oe)
                                    {
                                        Console.WriteLine(oe.Message);
                                        continue;
                                    }
                                    
                                    // print fullfilled order                                        
                                    var items = order.GetOrderSummary();
                                    items.ForEach(item => Console.WriteLine(item));       

                                    if(!order.IsOrderComplete && order.UnfulfilledQuantity > 0)
                                    {
                                        Console.WriteLine("System is unable to fulfill order with this remaining quantity {0}. ");
                                        Console.WriteLine("Please consider to re-order with a different quantity. We are sorry for the inconvenience");
                                    }
                                }
                            }
                        }
                    }
                }
                catch(Exception ex)
                {                    
                    Console.WriteLine("Error has occured. Please try again");
                    Console.WriteLine(ex.Message);
                }
            }
        }
    }
}
