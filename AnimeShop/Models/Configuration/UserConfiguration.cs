using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace AnimeShop.Models.Configuration
{
	public class UserConfiguration: IEntityTypeConfiguration<User>
    {
        public void Configure(EntityTypeBuilder<User> builder)
        {
            builder.HasData(new List<User> {
                new User {Id=1
                            ,Login="Vitoria"
                            ,FirstName="Vivi"
                            ,SurName="Olei"
                            ,Password="3214545454DIma"
                            ,Phone="89209972501"
                            ,TypeOfAcc="Admin"
                            ,Email="olegnikod3@gmail.com"}
                            });

            builder
                .Property(x => x.Login)
                .IsRequired();
            builder
                .Property(x => x.Login)
                .HasMaxLength(25);
            builder
                .Property(x => x.Phone)
                .IsRequired();

            builder
                .Property(x => x.TypeOfAcc)
                .IsRequired();
        }
    }
}
