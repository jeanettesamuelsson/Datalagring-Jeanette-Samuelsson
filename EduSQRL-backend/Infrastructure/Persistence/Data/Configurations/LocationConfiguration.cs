using Infrastructure.Persistence.Entities;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;



namespace Infrastructure.Persistence.Data.Configurations;

public class LocationConfiguration : IEntityTypeConfiguration<LocationEntity>
{
    public void Configure(EntityTypeBuilder<LocationEntity> builder)
    {
        builder.ToTable("Locations");

        builder.HasKey(e => e.Id).HasName("PK_Locations_Id");

        // properties
        

        builder.Property(e => e.Id)
            .HasDefaultValueSql("(NEWSEQUENTIALID())", "DF_Location_Id");

        builder.Property(e => e.Name)
            .HasMaxLength(100)
            .IsRequired();

        builder.Property(e => e.Concurrency)
            .IsRowVersion()
            .IsConcurrencyToken()
            .IsRequired();

        builder.Property(e => e.Created)
            .HasPrecision(0)
            .IsRequired()
            .HasDefaultValueSql("(SYSUTCDATETIME())", "DF_Locations_Created")
            .ValueGeneratedOnAdd();

        builder.Property(e => e.Modified)
            .HasPrecision(0)
            .IsRequired()
            .HasDefaultValueSql("(SYSUTCDATETIME())", "DF_Locations_Modified")
            .ValueGeneratedOnAddOrUpdate();

    }
}
