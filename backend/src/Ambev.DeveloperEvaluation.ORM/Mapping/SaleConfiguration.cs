using Ambev.DeveloperEvaluation.Domain.Entities;
using Ambev.DeveloperEvaluation.Domain.Enums;
using Ambev.DeveloperEvaluation.Domain.ValueObjects;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace Ambev.DeveloperEvaluation.ORM.Mapping;

public class SaleConfiguration : IEntityTypeConfiguration<Sale>
{
    public void Configure(EntityTypeBuilder<Sale> builder)
    {
        builder.ToTable("sales");

        builder.HasKey(s => s.Id);

        builder.Property(s => s.Id)
            .ValueGeneratedNever()
            .HasColumnName("id");

        builder.Property(s => s.SaleNumber)
            .HasColumnName("sale_number")
            .IsRequired()
            .HasMaxLength(50);

        builder.Property(s => s.CustomerId)
            .HasColumnName("customer_id")
            .IsRequired();

        builder.Property(s => s.CustomerName)
            .HasColumnName("customer_name")
            .IsRequired()
            .HasMaxLength(200);

        builder.Property(s => s.BranchId)
            .HasColumnName("branch_id")
            .IsRequired();

        builder.Property(s => s.BranchName)
            .HasColumnName("branch_name")
            .IsRequired()
            .HasMaxLength(200);

        builder.Property(s => s.Status)
            .HasColumnName("status")
            .HasConversion<int>()
            .HasSentinel(SaleStatus.None)
            .IsRequired();

        builder.Property(s => s.SaleDate)
            .HasColumnName("sale_date")
            .IsRequired();

        builder.Property(s => s.UpdatedAt)
            .HasColumnName("updated_at");

        builder.OwnsMany(s => s.Items, sa =>
        {
            sa.ToTable("sale_items");

            sa.WithOwner()
                .HasForeignKey("sale_id");

            sa.HasKey(l => l.Id);

            sa.Property(i => i.Id)
                .HasColumnName("id")
                .ValueGeneratedNever();

            sa.Property(i => i.Status)
                .HasColumnName("status")
                .HasConversion<int>()
                .HasSentinel(SaleItemStatus.None)
                .IsRequired();

            sa.Property(i => i.ProductId)
                .HasColumnName("product_id")
                .IsRequired();

            sa.Property(i => i.ProductName)
                .HasColumnName("product_name")
                .IsRequired()
                .HasMaxLength(200);

            sa.Property(i => i.Quantity)
                .HasColumnName("quantity")
                .IsRequired();

            sa.Property(s => s.UpdatedAt)
                .HasColumnName("updated_at");
            
            sa.Property(i => i.UnitPrice)
                .HasColumnName("unit_price")
                .HasConversion(
                    v => v.Value,
                    v => Money.Create(v)
                )
                .IsRequired();

            sa.Property(i => i.DiscountPercentage)
                .HasColumnName("discount_percentage")
                .HasConversion(
                    v => v.Value,
                    v => Percentage.Create(v)
                )
                .IsRequired();
        });
    }
}