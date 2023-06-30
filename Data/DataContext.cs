using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace dotnet_rpg.Data
{
    public class DataContext : DbContext
    {
      //constractor
      public DataContext(DbContextOptions<DataContext> options):base(options)
      {

      }
      protected override void OnModelCreating(ModelBuilder modelBuilder)
      {
          modelBuilder.Entity<Skill>().HasData(
            new Skill {Id = 1, Name="Fireball", Damage=30},
            new Skill {Id = 2, Name="Freezing", Damage=20},
            new Skill {Id = 3, Name="Blazor", Damage=50}
          );
        }
        // create table in DB
  
        // create table in DB
      public DbSet<Character> Characters => Set<Character>();
      public DbSet<User> Users => Set<User>();

      public DbSet<Weapon> Weapons => Set<Weapon>();
      public DbSet<Skill> Skills => Set<Skill>();

    }
}