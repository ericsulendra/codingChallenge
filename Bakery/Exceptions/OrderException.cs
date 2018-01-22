namespace CodingChallenge.Exceptions
{
    using System;
    public class OrderException : Exception
    {       
        public OrderException(string message)
            : base(message)
        {
        }
    }
}