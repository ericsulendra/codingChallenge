namespace Models
{
    using System;

    public class Product
    {
        public Guid id {get;set;}
        public string ProductName {get;set;}
        public string ProductCode {get;set;}
        public decimal UnitPrice {get;set;}
    }
}