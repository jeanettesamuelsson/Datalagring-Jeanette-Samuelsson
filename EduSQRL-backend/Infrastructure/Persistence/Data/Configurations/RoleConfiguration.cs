using Infrastructure.Persistence.Entities;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace Infrastructure.Persistence.Data.Configurations;

public class RoleConfiguration : IEntityTypeConfiguration<RoleEntity>
{
    public void Configure(EntityTypeBuilder<RoleEntity> builder)
    {
        builder.ToTable("Roles");

        // set PK

        builder.HasKey(e => e.Id).HasName("PK_Roles_Id");

        //set unique ID in database when added

        builder.Property(e => e.Id)
        .ValueGeneratedOnAdd()
        .HasDefaultValueSql("(NEWSEQUENTIALID())", "DF_Role_Id");


        builder.Property(e => e.RoleName)
        .IsRequired()
        .HasMaxLength(20);

        builder.HasIndex(e => e.RoleName, "UQ_Roles_RoleName").IsUnique();

     
        builder.Property(e => e.Concurrency)
        .IsRowVersion()
        .IsConcurrencyToken()
        .IsRequired();

        builder.Property(e => e.Created)
        .HasPrecision(0)
        .IsRequired()
        .HasDefaultValueSql("(SYSUTCDATETIME())", "DF_Role_Created")
        .ValueGeneratedOnAdd();

        builder.Property(e => e.Modified)
        .HasPrecision(0)
        .IsRequired()
        .HasDefaultValueSql("(SYSUTCDATETIME())", "DF_Role_Modified")
        .ValueGeneratedOnAddOrUpdate();

        // 1 - M Role -> Participants

        builder.HasMany(r => r.Participants) //one role has many participants
            .WithOne(p => p.Role) // one role has many participants
            .HasForeignKey(p => p.RoleId)
            .OnDelete(DeleteBehavior.Restrict)
            .HasConstraintName("FK_Participants_Roles");


        // seed data for Roles

        builder.HasData(
            new RoleEntity
            {
                Id = Guid.Parse("a1b2c3d4-e5f6-4a5b-8c9d-0e1f2a3b4c5d"),
                RoleName = "Student",
                Created = new DateTime(2026, 1, 1, 0, 0, 0, DateTimeKind.Utc)

            },
            new RoleEntity
            {
                Id = Guid.Parse("f9e8d7c6-b5a4-4f3e-2d1c-0b9a8f7e6d5c"),
                RoleName = "Teacher",
                Created = new DateTime(2026, 1, 1, 0, 0, 0, DateTimeKind.Utc)
            }
        );
    }
}








