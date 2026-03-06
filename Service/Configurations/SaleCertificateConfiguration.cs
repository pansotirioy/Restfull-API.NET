using Assignment.Models;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace Assignment.Service.Configurations
{
    public class SaleCertificateConfiguration : IEntityTypeConfiguration<SaleCertificate>
    {
        public void Configure(EntityTypeBuilder<SaleCertificate> entity)
        {

            entity.HasKey(e => e.Id);

            entity.Property(e => e.Price)
            .IsRequired();

        }
    }
}
