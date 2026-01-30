using Infrastructure.Entities;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace Infrastructure.Data.Configurations;

public class CourseSessionConfiguration : IEntityTypeConfiguration<CourseSessionEntity>
{
    public void Configure(EntityTypeBuilder<CourseSessionEntity> builder)
    {
        builder.ToTable("CourseSessions");

        builder.HasKey(e => e.Id).HasName("PK_CourseSessions_Id");

        builder.Property(e => e.Id)
            .HasDefaultValueSql("(NEWSEQUENTIALID())", "DF_CourseSession_Id");

        builder.Property(e => e.StartDate)
            .IsRequired();

        builder.Property(e => e.EndDate)
            .IsRequired();

        builder.Property(e => e.Capacity)
            .IsRequired();


        builder.Property(e => e.Concurrency)
         .IsRowVersion()
         .IsConcurrencyToken()
         .IsRequired();

        builder.Property(e => e.Created)
        .HasPrecision(0)
        .IsRequired()
        .HasDefaultValueSql("(SYSUTCDATETIME())", "DF_Course_Sessions_Created")
        .ValueGeneratedOnAdd();

        builder.Property(e => e.Modified)
        .HasPrecision(0)
        .IsRequired()
        .HasDefaultValueSql("(SYSUTCDATETIME())", "DF_Course_Sessions_Modified")
        .ValueGeneratedOnAddOrUpdate();
    }
}