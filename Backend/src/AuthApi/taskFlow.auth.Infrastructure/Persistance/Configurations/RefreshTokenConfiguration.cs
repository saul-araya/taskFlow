
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using taskFlow.auth.Domain.Entities;

namespace taskFlow.auth.Infrastructure.Persistance.Configurations;

public class RefreshTokenConfiguration : IEntityTypeConfiguration<RefreshToken>
{
    public void Configure(EntityTypeBuilder<RefreshToken> builder)
    {
        builder.ToTable("refresh_token");

        builder.HasKey(x => x.Id);

        builder.Property(x => x.Id)
            .HasColumnName("id")
            .IsRequired();

        builder.Property(x => x.UserId)
            .HasColumnName("user_id")
            .IsRequired();

        builder.Property(x => x.RefreshTokenHash)
            .HasColumnName("refresh_token_hash")
            .HasColumnType("bytea")
            .IsRequired();

        builder.Property(x => x.CreatedAt)
            .HasColumnName("created_at")
            .IsRequired();

        builder.Property(x => x.ExpiresAt)
            .HasColumnName("expires_at")
            .IsRequired();

        builder.Property(x => x.IsActive)
            .HasColumnName("is_active")
            .IsRequired();

        //References
        builder.HasOne(x => x.User)
            .WithMany(u => u.RefreshTokens)
            .HasForeignKey(x => x.UserId)
            .HasConstraintName("FK_REFRSH_TOKEN_USER");

        //Indexs
        builder.HasIndex(x => x.RefreshTokenHash)
            .HasFilter("\"is_active\" = true")
            .HasDatabaseName("IX_REFRESH_TOKEN_ACTIVE")
            .IsUnique(true);
        
    }
}