using Infrastructure.Entities;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace Infrastructure.Data.Configurations;

public class RoleConfiguration : IEntityTypeConfiguration<RoleEntity>
{
    public void Configure(EntityTypeBuilder<RoleEntity> entity)
    {
        entity.ToTable("Roles");

        entity.HasKey(e => e.Id).HasName("PK_Roles_Id");
        entity.Property(e => e.RoleName)
                .IsRequired()
                .HasMaxLength(20);

        entity.HasIndex(e => e.RoleName, "UQ_Roles_RoleName").IsUnique();

        entity.Property(e => e.Concurrency)
       .IsRowVersion()
       .IsConcurrencyToken()
       .IsRequired();

        entity.Property(e => e.Created)
        .HasPrecision(0)
        .IsRequired()
        .HasDefaultValueSql("(SYSUTCDATETIME())", "DF_Role_Created")
        .ValueGeneratedOnAdd();

        entity.Property(e => e.Modified)
        .HasPrecision(0)
        .IsRequired()
        .HasDefaultValueSql("(SYSUTCDATETIME())", "DF_Role_Modified")
        .ValueGeneratedOnAddOrUpdate();

        //set M-M relation between Participant and Roles

        entity.HasMany(r => r.Participants)
        .WithMany(p => p.Roles)
        .UsingEntity<Dictionary<string, object>>(
        "ParticipantRole",
        p => p.HasOne<ParticipantEntity>().WithMany().HasForeignKey("ParticipantId").OnDelete(DeleteBehavior.ClientSetNull),
        r => r.HasOne<RoleEntity>().WithMany().HasForeignKey("RoleId").OnDelete(DeleteBehavior.ClientSetNull),

        // set junction table name and keys
        j =>
        {
            j.HasKey("ParticipantId", "RoleId");
            j.ToTable("ParticipantRoles");
        }
        );

    }
}








