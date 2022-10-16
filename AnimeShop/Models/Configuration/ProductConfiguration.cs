using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using System.Reflection.Emit;

namespace AnimeShop.Models
{
    public class ProductConfiguration : IEntityTypeConfiguration<Product>
    {
        public void Configure(EntityTypeBuilder<Product> builder)
        {
            builder
                .Property(x => x.Title)
                .IsRequired();

            builder
                .Property(x => x.Title)
                .HasMaxLength(50);


            builder
                .Property(x => x.Description)
                .HasMaxLength(300);
            builder
                .Property(x => x.Description)
                .IsRequired();
        }
    }
}
