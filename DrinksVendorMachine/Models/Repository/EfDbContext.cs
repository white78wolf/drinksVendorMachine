using System.Data.Entity;

namespace DrinksVendorMachine.Models.Repository
{
    public class EfDbContext : DbContext
    {
        public DbSet<Drink> Drinks { get; set; }
        public DbSet<Coin> Coins { get; set; }
    }
}