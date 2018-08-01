using System;

namespace CartApi.Infrastructure.Exceptions
{
    public class CartDomainException : Exception
    {
        public CartDomainException()
        { }

        public CartDomainException(string message)
            :base(message)
        { }
        
        public CartDomainException(string message, Exception innerException)
            : base(message, innerException)
        { }
    }
}