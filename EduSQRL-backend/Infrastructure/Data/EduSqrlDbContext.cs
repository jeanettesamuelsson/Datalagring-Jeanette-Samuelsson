

using Infrastructure.Entities;
using Microsoft.EntityFrameworkCore;

namespace Infrastructure.Data;
public sealed class EduSqrlDbContext(DbContextOptions<EduSqrlDbContext> options) : DbContext(options)
{
    public DbSet<ParticipantEntity> Participants => Set<ParticipantEntity>();
    public DbSet<RoleEntity> Roles => Set<RoleEntity>();


    protected override void OnModelCreating(ModelBuilder modelBuilder)
    {
        modelBuilder.Entity<ParticipantEntity>(entity =>
        {
            entity.ToTable("Participants");

            entity.HasKey(e => e.Id).HasName("PK_Participants_Id");

            //properties

            //set unique ID in database when added

            entity.Property(e => e.Id)
            .ValueGeneratedOnAdd()
            .HasDefaultValueSql("(NEWSEQUENTIALID())", "DF_Participant_Id");

            entity.Property(e => e.FirstName)
            .HasMaxLength(50)
            .IsRequired();

            entity.Property(e => e.LastName)
            .HasMaxLength(50)
            .IsRequired();

            entity.Property(e => e.Email)
            .HasMaxLength(256)
            .IsRequired();

            entity.Property(e => e.PhoneNumber)
            .HasMaxLength(13)
            .IsUnicode(false)
            .IsRequired(false);

            entity.Property(e => e.Concurrency)
            .IsRowVersion()
            .IsConcurrencyToken()
            .IsRequired();

            entity.Property(e => e.Created)
            .HasPrecision(0)
            .IsRequired()
            .HasDefaultValueSql("(SYSUTCDATETIME())", "DF_Participants_Created")
            .ValueGeneratedOnAdd();

            entity.Property(e => e.Modified)
            .HasPrecision(0)
            .IsRequired()
            .HasDefaultValueSql("(SYSUTCDATETIME())", "DF_Participants_Modified")
            .ValueGeneratedOnAddOrUpdate();


            entity.HasIndex(e => e.Email, "UQ_Participants_Email").IsUnique();
            entity.ToTable(tb => tb.HasCheckConstraint("CK_Participants_Email_NotEmpty", "LTRIM(RTRIM([Email])) <> ''"));

        });

        modelBuilder.Entity<RoleEntity>(entity =>
        {

            entity.ToTable("Roles");

            entity.HasKey(e => e.Id).HasName("PK_Roles_Id");
            entity.Property(e => e.RoleName)
            .IsRequired()
            .HasMaxLength(20);

            entity.HasIndex(e => e.RoleName, "UQ_Roles_RoleName").IsUnique();
        });

        //set M-M relation between Participant and Roles

        modelBuilder.Entity<ParticipantEntity>()
            .HasMany(p => p.Roles)
            .WithMany(r => r.Participants)


            .UsingEntity<Dictionary<string, object>>(
            "ParticipantRole",
            r => r.HasOne<RoleEntity>().WithMany().HasForeignKey("RoleId").OnDelete(DeleteBehavior.ClientSetNull),
            p => p.HasOne<ParticipantEntity>().WithMany().HasForeignKey("ParticipantId").OnDelete(DeleteBehavior.ClientSetNull),

            e =>
            {
                e.HasKey("ParticipantId", "RoleId");
                e.ToTable("ParticipantRoles");
            }
            );
            
           
              
    }

}

