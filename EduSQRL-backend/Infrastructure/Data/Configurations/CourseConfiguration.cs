using Infrastructure.Entities;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace Infrastructure.Data.Configurations;

public class CourseConfiguration : IEntityTypeConfiguration<CourseEntity>
{
    public void Configure(EntityTypeBuilder<CourseEntity> entity)
    {
        entity.ToTable("Courses");

        //set PK

        entity.HasKey(e => e.Id).HasName("PK_Courses_Id");

        //set unique ID in database when added

        entity.Property(e => e.Id)
        .ValueGeneratedOnAdd()
        .HasDefaultValueSql("(NEWSEQUENTIALID())", "DF_Course_Id");

        entity.Property(e => e.CourseName)
        .HasMaxLength(50)
        .IsRequired();

        entity.Property(e => e.CourseCode)
        .HasMaxLength(50)
        .IsRequired();

        entity.Property(e => e.Description)
        .HasMaxLength(150)
        .IsRequired();

        entity.Property(e => e.Concurrency)
        .IsRowVersion()
        .IsConcurrencyToken()
        .IsRequired();

        entity.Property(e => e.Created)
        .HasPrecision(0)
        .IsRequired()
        .HasDefaultValueSql("(SYSUTCDATETIME())", "DF_Courses_Created")
        .ValueGeneratedOnAdd();

        entity.Property(e => e.Modified)
        .HasPrecision(0)
        .IsRequired()
        .HasDefaultValueSql("(SYSUTCDATETIME())", "DF_Courses_Modified")
        .ValueGeneratedOnAddOrUpdate();


        //Add relation to course_session

        entity.HasMany(c => c.CourseSessions)
           .WithOne(s => s.Course)
           .HasForeignKey(s => s.CourseId);

    }
}
