using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using OrderApi.Domain.Models;

namespace OrderApi.Domain.Data.Mapping
{
    public class OrderItemMap : IEntityTypeConfiguration<OrderItem>
    {
        public void Configure(EntityTypeBuilder<OrderItem> builder)
        {
            builder.ToTable("ORDER_ITEM");
            builder.HasKey(p => p.Id);

            builder.Property(p => p.PictureUrl).HasColumnName("PictureUrl").HasMaxLength(100);
            builder.Property(p => p.ProductId).HasColumnName("ProductId");
            builder.Property(p => p.ProductName).HasColumnName("ProductName").HasMaxLength(100);
            builder.Property(p => p.UnitPrice).HasColumnName("UnitPrice");
            builder.Property(p => p.Units).HasColumnName("Units");
            builder.Property(p => p.OrderId).HasColumnName("OrderId");

            builder.HasOne(p => p.Order).WithMany(p => p.OrderItems).HasForeignKey(p => p.OrderId);
        }
        
    }
}