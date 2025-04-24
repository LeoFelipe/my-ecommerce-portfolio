using EcommercePortfolio.Domain.Orders.Entities;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using Microsoft.EntityFrameworkCore;

namespace EcommercePortfolio.Infra.Data.Orders.Mappings;

public class OrderItemMapping : IEntityTypeConfiguration<OrderItem>
{
    public void Configure(EntityTypeBuilder<OrderItem> builder)
    {
        builder.ToTable("order_item");

        builder.HasKey(c => c.Id);

        builder.Property(c => c.OrderId)
            .IsRequired()
            .HasColumnName("order_id");

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
            .WithMany(c => c.OrderItems);

    }
}