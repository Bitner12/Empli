using Domain.Entities;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace Infrastructures.Contexts.Configartion;

public class WorkerConfiguration : IEntityTypeConfiguration<Worker>
{
    public void Configure(EntityTypeBuilder<Worker> builder)
    {
        builder.HasKey(x => x.Id);
        builder.HasIndex(w => w.Pesel);

        builder.HasMany(w => w.Hours)
            .WithOne(h => h.Worker)
            .HasForeignKey(h => h.WorkerId);

    }
}