using Infrastructure.Entities;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace Infrastructure.Data.Configurations;

public class CourseSessionConfiguration : IEntityTypeConfiguration<CourseSessionEntity>
{
    public void Configure(EntityTypeBuilder<CourseSessionEntity> entity)
    {
        entity.ToTable("CourseSessions");

        entity.HasKey(e => e.Id).HasName("PK_CourseSessions_Id");

        entity.Property(e => e.Id)
            .HasDefaultValueSql("(NEWSEQUENTIALID())", "DF_CourseSession_Id");

        entity.Property(e => e.StartDate)
            .IsRequired(); 

        entity.Property(e => e.EndDate)
            .IsRequired(); 

        entity.Property(e => e.Capacity)
            .IsRequired();


        entity.Property(e => e.Concurrency)
         .IsRowVersion()
         .IsConcurrencyToken()
         .IsRequired();

        entity.Property(e => e.Created)
        .HasPrecision(0)
        .IsRequired()
        .HasDefaultValueSql("(SYSUTCDATETIME())", "DF_Course_Sessions_Created")
        .ValueGeneratedOnAdd();

        entity.Property(e => e.Modified)
        .HasPrecision(0)
        .IsRequired()
        .HasDefaultValueSql("(SYSUTCDATETIME())", "DF_Course_Sessions_Modified")
        .ValueGeneratedOnAddOrUpdate();
    }
}