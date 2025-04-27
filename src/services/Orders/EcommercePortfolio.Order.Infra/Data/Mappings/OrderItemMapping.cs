using Microsoft.EntityFrameworkCore.Metadata.Builders;
using Microsoft.EntityFrameworkCore;
using EcommercePortfolio.Orders.Domain.Entities;

namespace EcommercePortfolio.Orders.Infra.Data.Mappings;

public class OrderItemMapping : IEntityTypeConfiguration<OrderItem>
{
    public void Configure(EntityTypeBuilder<OrderItem> builder)
    {
        builder.ToTable("order_item");

        builder.HasKey(c => c.Id);

        builder.Property(c => c.OrderId)
            .HasColumnName("order_id")
            .IsRequired();

        builder.Property(c => c.ProductId)
            .IsRequired()
            .HasColumnName("product_id")
            .HasColumnType("int");

        builder.Property(c => c.ProductName)
            .IsRequired()
            .HasColumnName("product_name")
            .HasColumnType("varchar(100)");

        builder.Property(c => c.Category)
            .IsRequired()
            .HasColumnName("category")
            .HasColumnType("varchar(100)");

        builder.Property(c => c.Quantity)
            .IsRequired()
            .HasColumnName("quantity")
            .HasColumnType("int");

        builder.Property(c => c.Price)
            .IsRequired()
            .HasColumnName("price")
            .HasColumnType("decimal(18,2)");

        builder.HasOne(c => c.Order)
            .WithMany(c => c.OrderItems)
            .IsRequired();
    }
}