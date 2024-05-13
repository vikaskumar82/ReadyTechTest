using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Design;
using ReadyTechCoffee.Model;
using System.Collections.Generic;

namespace ReadyTechCoffee.Model
{
    public class CoffeeContext : DbContext
    {

        protected override void OnConfiguring(DbContextOptionsBuilder options)
        {
            if (!options.IsConfigured)
            {
                options.UseSqlServer("Data Source=CASPER;User ID=root;Password=Password99;Initial Catalog=coffeedb;Trust Server Certificate=True");
            }
        }

        public CoffeeContext(DbContextOptions<CoffeeContext> options) : base(options)
        {

        }

        public virtual DbSet<CoffeeOrderItem> CoffeeOrders { get; set; }
    }

    public class YourDbContextFactory : IDesignTimeDbContextFactory<CoffeeContext>
    {
        public CoffeeContext CreateDbContext(string[] args)
        {
            var optionsBuilder = new DbContextOptionsBuilder<CoffeeContext>();
            optionsBuilder.UseSqlServer("Data Source=CASPER;User ID=root;Password=Password99;Initial Catalog=coffeedb;Trust Server Certificate=True");

            return new CoffeeContext(optionsBuilder.Options);
        }
    }
}
