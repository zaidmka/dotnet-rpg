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
        // create table in DB
      public DbSet<Character> Characters => Set<Character>();
      public DbSet<User> Users => Set<User>();

    }
}