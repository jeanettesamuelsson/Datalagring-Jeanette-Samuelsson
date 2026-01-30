using Infrastructure.Entities;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace Infrastructure.Data.Configurations;

public class CourseConfiguration : IEntityTypeConfiguration<CourseEntity>
{
    public void Configure(EntityTypeBuilder<CourseEntity> builder)
    {
        builder.ToTable("Courses");

        //set PK

        builder.HasKey(e => e.Id).HasName("PK_Courses_Id");

        //set unique ID in database when added

        builder.Property(e => e.Id)
        .ValueGeneratedOnAdd()
        .HasDefaultValueSql("(NEWSEQUENTIALID())", "DF_Course_Id");

        builder.Property(e => e.CourseName)
        .HasMaxLength(50)
        .IsRequired();

        builder.Property(e => e.CourseCode)
        .HasMaxLength(50)
        .IsRequired();

        builder.Property(e => e.Description)
        .HasMaxLength(150)
        .IsRequired();

        builder.Property(e => e.Concurrency)
        .IsRowVersion()
        .IsConcurrencyToken()
        .IsRequired();

        builder.Property(e => e.Created)
        .HasPrecision(0)
        .IsRequired()
        .HasDefaultValueSql("(SYSUTCDATETIME())", "DF_Courses_Created")
        .ValueGeneratedOnAdd();

        builder.Property(e => e.Modified)
        .HasPrecision(0)
        .IsRequired()
        .HasDefaultValueSql("(SYSUTCDATETIME())", "DF_Courses_Modified")
        .ValueGeneratedOnAddOrUpdate();


        //Add relation to course_session

        builder.HasMany(c => c.CourseSessions)
           .WithOne(s => s.Course)
           .HasForeignKey(s => s.CourseId);

    }
}
