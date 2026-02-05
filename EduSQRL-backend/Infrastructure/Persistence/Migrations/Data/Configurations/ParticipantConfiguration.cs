using Infrastructure.Persistence.Entities;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace Infrastructure.Persistence.Migrations.Data.Configurations;

public class ParticipantConfiguration : IEntityTypeConfiguration<ParticipantEntity>
{
    public void Configure(EntityTypeBuilder<ParticipantEntity> builder
        )
    {
        builder.ToTable("Participants");

        builder.HasKey(e => e.Id).HasName("PK_Participants_Id");

        //properties

        //set unique ID in database when added

        builder.Property(e => e.Id)
        .ValueGeneratedOnAdd()
        .HasDefaultValueSql("(NEWSEQUENTIALID())", "DF_Participant_Id");

        builder.Property(e => e.FirstName)
        .HasMaxLength(50)
        .IsRequired();

        builder.Property(e => e.LastName)
        .HasMaxLength(50)
        .IsRequired();

        builder.Property(e => e.Email)
        .HasMaxLength(256)
        .IsRequired();

        builder.Property(e => e.PhoneNumber)
        .HasMaxLength(13)
        .IsUnicode(false)
        .IsRequired(false);

        builder.Property(e => e.Concurrency)
        .IsRowVersion()
        .IsConcurrencyToken()
        .IsRequired();

        builder.Property(e => e.Created)
        .HasPrecision(0)
        .IsRequired()
        .HasDefaultValueSql("(SYSUTCDATETIME())", "DF_Participants_Created")
        .ValueGeneratedOnAdd();

        builder.Property(e => e.Modified)
        .HasPrecision(0)
        .IsRequired()
        .HasDefaultValueSql("(SYSUTCDATETIME())", "DF_Participants_Modified")
        .ValueGeneratedOnAddOrUpdate();


        builder.HasIndex(e => e.Email, "UQ_Participants_Email").IsUnique();
        builder.ToTable(tb => tb.HasCheckConstraint("CK_Participants_Email_NotEmpty", "LTRIM(RTRIM([Email])) <> ''"));

    }
}

