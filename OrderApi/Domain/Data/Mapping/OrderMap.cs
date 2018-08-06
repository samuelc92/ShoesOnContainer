using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using OrderApi.Domain.Models;

namespace OrderApi.Domain.Data.Mapping
{
    public class OrderMap : IEntityTypeConfiguration<Order>
    {
        public void Configure(EntityTypeBuilder<Order> builder)
        {
            builder.ToTable("ORDER");
            builder.HasKey(p => p.OrderId);

            builder.Property(p => p.Address).HasColumnName("Address").HasMaxLength(100);
            builder.Property(p => p.FirstName).HasColumnName("FirstName").HasMaxLength(100);
            builder.Property(p => p.LastName).HasColumnName("LastName").HasMaxLength(100);
            builder.Property(p => p.OrderDate).HasColumnName("OrderDate").IsRequired();
            builder.Property(p => p.OrderStatus).HasColumnName("OrderStatus");
            builder.Property(p => p.OrderTotal).HasColumnName("OrderTotal").IsRequired();
            builder.Property(p => p.PaymentAuthCode).HasColumnName("PaymentAuthCode").HasMaxLength(100);
            builder.Property(p => p.UserName).HasColumnName("UserName").HasMaxLength(100);
        }
    }
}