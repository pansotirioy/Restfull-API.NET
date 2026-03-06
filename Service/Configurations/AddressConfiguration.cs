using Assignment.Models;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace Assignment.Service.Configurations
{
    public class AddressConfiguration : IEntityTypeConfiguration<Address>
    {
        public void Configure(EntityTypeBuilder<Address> entity)
        {

            entity.HasKey(e => e.Id);

            entity.Property(e => e.City)
                .IsRequired();

            entity.Property(e => e.Country)
            .IsRequired();

            entity.Property(e => e.State)
            .IsRequired();

            entity.Property(e => e.Street)
            .IsRequired();

            entity.Property(e => e.LandlineNumber)
            .IsRequired();

            entity.Property(e => e.PostalCode)
            .IsRequired()
            .HasMaxLength(5);

            entity.HasOne(e => e.Candidate)
                .WithOne(e => e.Address)
                .HasForeignKey<Address>(e => e.CandidateId);

        }
    }
}
