namespace CodingChallenge.Tests.DataStore
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
            packages.Add(new Package {
                UnitPrice = 9.95m,
                PackSize = 7,
                ProductCode = "MB11"
            });
            packages.Add(new Package {
                UnitPrice = 16.95m,
                PackSize = 11,
                ProductCode = "MB11"
            });
            packages.Add(new Package {
                UnitPrice = 24.95m,
                PackSize = 13,
                ProductCode = "MB11"
            });
            packages.Add(new Package {
                UnitPrice = 24.95m,
                PackSize = 17,
                ProductCode = "MB11"
            });
            return packages;
        }

        public List<Product> GetProducts()
        {
            throw new System.NotImplementedException();
        }
    }
}