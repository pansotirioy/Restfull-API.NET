using Assignment.Models;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace Assignment.Service.Configurations
{
    public class QuestionConfiguration : IEntityTypeConfiguration<Question>
    {
        public void Configure(EntityTypeBuilder<Question> entity)
        {

            entity.HasKey(e => e.Id);

            entity.Property(e => e.question)
                  .IsRequired();

            entity.Property(e => e.answer1)
                  .IsRequired();

            entity.Property(e => e.answer2)
                  .IsRequired();

            entity.Property(e => e.answer3)
                  .IsRequired();

            entity.Property(e => e.answer4)
                  .IsRequired();

            entity.Property(e => e.correct)
                  .IsRequired();

            entity.Property(e => e.correct)
                  .IsRequired();

            entity.HasOne(e => e.candidatesAnalytics)
                .WithMany(e => e.Questions)
                .HasForeignKey(e => e.CandidatesAnalyticsId)
                .IsRequired(false);

        }
    }
}
