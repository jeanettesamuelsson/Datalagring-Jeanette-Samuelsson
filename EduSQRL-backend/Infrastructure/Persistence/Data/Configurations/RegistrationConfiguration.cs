using Infrastructure.Persistence.Entities;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;



namespace Infrastructure.Persistence.Data.Configurations;

public class RegistrationConfiguration : IEntityTypeConfiguration<RegistrationEntity>
{
    public void Configure(EntityTypeBuilder<RegistrationEntity> builder)
    {
        builder.ToTable("Registrations");

        // set PK

        builder.HasKey(e => e.Id).HasName("PK_Registrations_Id");

        // set unique ID in database when added

        builder.Property(e => e.Id)
        .ValueGeneratedOnAdd()
        .HasDefaultValueSql("(NEWSEQUENTIALID())", "DF_Registration_Id");

        // other properties

        builder.Property(e => e.Status)
        .IsRequired();

        builder.Property(e => e.ParticipantId)
        .IsRequired();

        builder.Property(e => e.CourseSessionId)
        .IsRequired();

        
        builder.Property(e => e.Concurrency)
        .IsRowVersion()
        .IsConcurrencyToken()
        .IsRequired();

        builder.Property(e => e.Created)
        .HasPrecision(0)
        .IsRequired()
        .HasDefaultValueSql("(SYSUTCDATETIME())", "DF_Registrations_Created")
        .ValueGeneratedOnAdd();

        builder.Property(e => e.Modified)
        .HasPrecision(0)
        .IsRequired()
        .HasDefaultValueSql("(SYSUTCDATETIME())", "DF_Registrations_Modified")
        .ValueGeneratedOnAddOrUpdate();

        // relationships

        // 1 - M participant -> registrations

        builder.HasOne(e => e.Participant)
        .WithMany(p => p.Registrations)
        .HasForeignKey(e => e.ParticipantId)
        .OnDelete(DeleteBehavior.Cascade)
        .HasConstraintName("FK_Registrations_Participants");

        // 1 - M courseSession -> registrations

        builder.HasOne(e => e.CourseSession)
        .WithMany(s => s.Registrations)
        .HasForeignKey(e => e.CourseSessionId)
        .OnDelete(DeleteBehavior.Cascade)
        .HasConstraintName("FK_Registrations_CourseSessions");

        // unique index for registration/participant/course session

        builder.HasIndex(e => new { e.ParticipantId, e.CourseSessionId }, "UQ_Registrations_Participant_Session")
        .IsUnique();
    }


    

}
