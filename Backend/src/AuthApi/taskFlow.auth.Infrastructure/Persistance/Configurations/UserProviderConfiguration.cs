
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using taskFlow.auth.Domain.Entities;

namespace taskFlow.auth.Infrastructure.Persistance.Configurations;

public class UserProviderConfiguration : IEntityTypeConfiguration<UserProvider>
{
    public void Configure(EntityTypeBuilder<UserProvider> builder)
    {
        builder.HasKey(x => x.Id);

        builder.Property(x => x.Id)
            .HasColumnName("id")
            .IsRequired();

        builder.Property(x => x.UserId)
            .HasColumnName("user_id")
            .HasColumnType("uuid")
            .IsRequired();

        builder.Property(x => x.Provider)
            .HasConversion<string>()
            .HasColumnName("provider")
            .HasMaxLength(50)
            .IsRequired();

        builder.Property(x => x.ProviderUserId)
            .HasColumnName("provider_user_id")
            .HasMaxLength(255);

        builder.Property(x => x.PasswordHash)
            .HasColumnName("password_hash")
            .HasMaxLength(255);

        //References
        builder.HasOne(x => x.User)
            .WithMany(x => x.UserProviders)
            .HasForeignKey(x => x.UserId)
            .OnDelete(DeleteBehavior.Cascade);

        //Indexs
        builder.HasIndex(x => x.UserId)
            .HasDatabaseName("IX_USER_PROVIDER_FK");

    }
}