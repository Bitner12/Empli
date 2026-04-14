using Domain.Entities;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace Infrastructures.Contexts.Configartion;

public class CompanyConfiguration : IEntityTypeConfiguration<Company>
{
    public void Configure(EntityTypeBuilder<Company> builder)
    {
        builder.HasKey(x => x.Id);
        builder.HasMany(c => c.Workers)
            .WithOne(w => w.Company)
            .HasForeignKey(w => w.CompanyId);
        
        
        builder.HasIndex(c => c.UserId).IsUnique();
        
    }
}