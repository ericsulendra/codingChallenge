namespace CodingChallenge.Models
{
    public class OrderItem
    {
        public int Quantity { get;set; }
        public string ProductCode { get;set; }
        public decimal TotalPrice { get;set; }

        public override string ToString()
        {
            return string.Format("{0} {1} {2}", Quantity, ProductCode, TotalPrice);
        }
    }
}
    
