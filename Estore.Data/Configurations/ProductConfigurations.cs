using Estore.Core.Entities;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace Estore.Data.Configurations
{
    internal class ProductConfigurations : IEntityTypeConfiguration<Product>
    {
        public void Configure(EntityTypeBuilder<Product> builder)
        {
            builder.Property(x => x.Name).IsRequired().HasMaxLength(50);
            builder.Property(x => x.Image).HasMaxLength(100);
            builder.Property(x => x.ProductCode).HasMaxLength(50);
            //FluentAPI ile classlar arası ilişki kurma
            builder.HasOne(x => x.Brand).WithMany(x => x.Products).HasForeignKey(f => f.BrandId); //Brand classı ile Product classı arasında 1e çok ilişki vardır
            builder.HasOne(x => x.Category).WithMany(x => x.Products).HasForeignKey(c => c.CategoryId); //Category classı ile Product classı arasında 1e çok ilişki vardır
        }
    }
}
