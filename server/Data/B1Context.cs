using System.Reflection;
using System.Linq;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations.Schema;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Internal;
using Microsoft.Extensions.Configuration;

using B.Models.B1;

namespace B.Data
{
  public partial class B1Context : Microsoft.EntityFrameworkCore.DbContext
  {
    public B1Context(DbContextOptions<B1Context> options):base(options)
    {
    }

    public B1Context()
    {
    }

    partial void OnModelBuilding(ModelBuilder builder);

    protected override void OnModelCreating(ModelBuilder builder)
    {
        base.OnModelCreating(builder);


        this.OnModelBuilding(builder);
    }


    public DbSet<B.Models.B1.Citizenship> Citizenships
    {
      get;
      set;
    }

    protected override void ConfigureConventions(ModelConfigurationBuilder configurationBuilder)
    {
        configurationBuilder.Conventions.Add(_ => new BlankTriggerAddingConvention());
    }
  }
}
