namespace Tests.DataStore
{
    using System.Collections.Generic;
    using CodingChallenge.Data;
    using CodingChallenge.Models;

    public class TestDataStore : IProductStore
    {
        public List<Package> GetPackages()
        {
            var packages = new List<Package>();
            packages.Add(new Package {
                UnitPrice = 6.99m,
                PackSize = 3,
                ProductCode = "VS5"
            });
            packages.Add(new Package {
                UnitPrice = 8.99m,
                PackSize = 5,
                ProductCode = "VS5"
            });
            return packages;
        }

        public List<Product> GetProducts()
        {
            throw new System.NotImplementedException();
        }
    }
}