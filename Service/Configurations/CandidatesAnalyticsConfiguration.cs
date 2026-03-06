using Assignment.Models;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace Assignment.Service.Configurations
{
    public class CandidatesAnalyticsConfiguration : IEntityTypeConfiguration<CandidatesAnalytics>
    {
        public void Configure(EntityTypeBuilder<CandidatesAnalytics> entity)
        {

            entity.HasKey(e => e.Id);

        }
    }
}
