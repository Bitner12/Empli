using Domain.Entities;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace Infrastructures.Contexts.Configartion;

public class UserConfiguration : IEntityTypeConfiguration<User>
{
    public void Configure(EntityTypeBuilder<User> builder)
    {
        builder.HasKey(u => u.Id);
        builder.Property(x => x.UserType)
            .IsRequired();

        builder.HasOne(u => u.Company)
            .WithOne(c => c.User)
            .HasForeignKey<Company>(c => c.UserId)
            .OnDelete(DeleteBehavior.Cascade);

        builder.HasOne(u => u.Employee)
            .WithOne(e => e.User)
            .HasForeignKey<Employee>(e => e.UserId)
            .OnDelete(DeleteBehavior.Cascade);

        builder.HasOne(r => r.RefreshToken)
            .WithOne(r => r.User)
            .HasForeignKey<RefreshToken>(r => r.UserId)
            .IsRequired(false)
            .OnDelete(DeleteBehavior.Cascade);
    }
}