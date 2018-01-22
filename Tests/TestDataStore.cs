namespace Tests.DataStore
{
    using System.Collections.Generic;
    using CodingChallenge.Data;
    using CodingChallenge.Models;

    public class TestDataStore : IProductStore
    {
        List<Package> IProductStore.GetPackages()
        {
            var packages = new List<Package>();
            packages.Add(new Package {
                UnitPrice = 6.99m,
                Quantity = 3,
                ProductCode = "VS5"
            });
            packages.Add(new Package {
                UnitPrice = 8.99m,
                Quantity = 5,
                ProductCode = "VS5"
            });
            return packages;
        }

        List<Product> IProductStore.GetProducts()
        {
            throw new System.NotImplementedException();
        }
    }
}