

using Infrastructure.Entities;
using Microsoft.EntityFrameworkCore;

namespace Infrastructure.Data;
public sealed class EduSqrlDbContext(DbContextOptions<EduSqrlDbContext> options) : DbContext(options)
{
    
    //dbSet
    public DbSet<ParticipantEntity> Participants => Set<ParticipantEntity>();
    public DbSet<RoleEntity> Roles => Set<RoleEntity>();


    protected override void OnModelCreating(ModelBuilder modelBuilder)
    {

        modelBuilder.ApplyConfigurationsFromAssembly(typeof(EduSqrlDbContext).Assembly);

       
    }

}

