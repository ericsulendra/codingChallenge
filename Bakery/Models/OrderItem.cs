namespace CodingChallenge.Models
{
    public class OrderItem
    {
        private int _quantity;
        private decimal _packPrice;
        private decimal _totalPrice;

        public int Quantity 
        { 
            get
            {
                return _quantity;
            }
            set
            {
                this._quantity = value;
                CalculateTotalPrice();
            }
        }
        public int PackSize { get;set; }
        public string ProductCode { get;set; }
        public decimal PackPrice 
        { 
            get 
            {
                return _packPrice;
            }    
            set
            {
                this._packPrice = value;
                CalculateTotalPrice();
            }
        }

        public decimal TotalPrice 
        { 
            get 
            {
                return _totalPrice;
            }
         }

        public override string ToString()
        {
            return string.Format("{0} x {1} {2} {3}", _quantity, PackSize, ProductCode, TotalPrice);
        }
        
        private void CalculateTotalPrice() 
        {            
            _totalPrice = _quantity * PackPrice;
        }
    }
}