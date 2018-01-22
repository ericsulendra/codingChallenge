namespace CodingChallenge.Data
{
    using System.Collections.Generic;
    using Models;
    public interface IProductStore 
    {
        List<Package> GetPackages();
        List<Product> GetProducts();
    }
}