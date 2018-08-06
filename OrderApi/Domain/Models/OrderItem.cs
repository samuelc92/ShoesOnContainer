using System;
using OrderApi.Infrastructure.Exceptions;

namespace OrderApi.Domain.Models
{
    public class OrderItem
    {
        public int Id { get; set; }
        public string ProductName { get; set; }
        public string PictureUrl { get; set; }
        public decimal UnitPrice { get; set; }
        public int Units { get; set; }
        public int ProductId { get; set; }
        public int OrderId { get; set; }
        public Order Order { get; set; }
        
        protected OrderItem()
        {
        }

        public OrderItem(int productId, string productName, decimal unitPrice, string pictureUrl
        , int units)
        {
            if (units <= 0)
            {
                throw new OrderingDomainException("Invalid number of units");
            }

            ProductId = productId;
            ProductName = productName;
            UnitPrice = unitPrice;
            PictureUrl = pictureUrl;
        }
        
        public void SetPictureUri(string pictureUri)
        {
            if (!String.IsNullOrWhiteSpace(pictureUri))
            {
                PictureUrl = pictureUri;
            }
        }
        
        public void AddUnits(int units)
        {
            if (units < 0)
            {
                throw new OrderingDomainException("Invalid units");
            }

            Units += units;
        }
    }
}