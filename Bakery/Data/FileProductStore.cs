namespace CodingChallenge.Data
{
    using System;
    using System.Collections.Generic;
    using System.IO;
    using Models;

    public class FileProductStore : IProductStore
    {
        private List<Product> products {get;set;}
        private List<Package> packages {get;set;}
        private string filePath;

        public FileProductStore(string filePath)
        {
            products = new List<Product>();
            packages = new List<Package>();
            this.filePath = filePath;
            ReadProductFile(filePath);
        }

        public List<Product> GetProducts() 
        {
            return products;
        }

        public List<Package> GetPackages() 
        {
            return packages;
        }

        private void ReadProductFile(string path)
        {
            using(var inputFile = File.OpenText(path))
            {
                string line;
                var isProduct = false;
                var isProductPack = false;
                while((line = inputFile.ReadLine()) != null)
                {
                    if(string.CompareOrdinal(line.Trim(), "#Product") == 0)
                    {
                        isProduct = true;
                        isProductPack = false;
                        continue;
                    }
                    else if(string.CompareOrdinal(line.Trim(), "#ProductPack") == 0)
                    {
                        isProduct = false;
                        isProductPack = true;
                        continue;
                    }

                    if(isProduct)
                    {
                        var splitProductArray = line.Split(new [] {','}, StringSplitOptions.RemoveEmptyEntries);
                        if(splitProductArray.Length == 2)
                        {
                            var product = new Product {
                                ProductCode = splitProductArray[0],
                                ProductName = splitProductArray[1]
                            };
                            products.Add(product);
                        }                        
                    }
                    else if(isProductPack)
                    {
                        var splitPackArray = line.Split(new [] {' '}, StringSplitOptions.RemoveEmptyEntries);

                        if(splitPackArray.Length == 3)
                        {
                            var package = new Package {
                                PackSize = int.Parse(splitPackArray[0]),
                                ProductCode = splitPackArray[1],
                                UnitPrice = decimal.Parse(splitPackArray[2])
                            };
                            packages.Add(package);
                        }
                    }
                }
            }
        }
    }
}