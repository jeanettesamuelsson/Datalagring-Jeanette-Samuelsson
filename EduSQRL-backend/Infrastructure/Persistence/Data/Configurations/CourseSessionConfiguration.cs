using Infrastructure.Persistence.Entities;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace Infrastructure.Persistence.Data.Configurations;

public class CourseSessionConfiguration : IEntityTypeConfiguration<CourseSessionEntity>
{
    public void Configure(EntityTypeBuilder<CourseSessionEntity> builder)
    {
        builder.ToTable("CourseSessions");

        // PK

        builder.HasKey(e => e.Id).HasName("PK_CourseSessions_Id");

        // set unique ID in database when added

        builder.Property(e => e.Id)
        .ValueGeneratedOnAdd()
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


        // relationships

        //Course
        builder.HasOne(s => s.Course)
       .WithMany(c => c.CourseSessions)
       .HasForeignKey(s => s.CourseId)
       .OnDelete(DeleteBehavior.Cascade); // will delete sessions connected to the course

        //Location
        builder.HasOne(s => s.Location)
        .WithMany(l => l.CourseSessions) 
        .HasForeignKey(s => s.LocationId)
        .OnDelete(DeleteBehavior.Cascade); // will delete sessions connected to the location


    }
}