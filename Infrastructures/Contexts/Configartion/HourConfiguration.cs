using Domain.Entities;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace Infrastructures.Contexts.Configartion;

public class HourConfiguration : IEntityTypeConfiguration<Hour>
{
    public void Configure(EntityTypeBuilder<Hour> builder)
    {
        builder.HasKey(h => h.Id);
        builder.HasOne(h => h.Worker)
            .WithMany(w => w.Hours)
            .HasForeignKey(h => h.WorkerId);
        builder.HasOne(h => h.Contractor)
            .WithMany(c => c.Hours)
            .HasForeignKey(h => h.ContractorId)
            .IsRequired(false)
            .OnDelete(DeleteBehavior.SetNull);
    }
    
}