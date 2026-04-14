using Domain.Entities;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;


namespace Infrastructures.Contexts.Configartion
{
    public class EmployeeConfiguration : IEntityTypeConfiguration<Employee>
    {
        public void Configure(EntityTypeBuilder<Employee> builder)
        {
            builder.HasKey(x => x.Id);
            builder.HasOne(e => e.User)
                .WithOne(u => u.Employee)
                .HasForeignKey<Employee>(e => e.UserId);
            builder.HasIndex(e => e.Pesel)
            .IsUnique();


            builder.HasOne(e => e.Worker)
                .WithOne(w => w.Employee)
                .HasForeignKey<Employee>(e => e.WorkerId);  

        }
    }
}
