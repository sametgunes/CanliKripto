using CanliKripto.Entities.Concretes;
using Microsoft.EntityFrameworkCore;
namespace CanliKripto.DbContexts
{
    public class CryptoCurrencyDbContext : DbContext
    {
        public CryptoCurrencyDbContext(DbContextOptions<CryptoCurrencyDbContext> options) : base(options)
        {
        }

        public DbSet<CryptoCurrency> CryptoCurrencies { get; set; }
        public DbSet<CryptoCurrencyValue> CryptoCurrencyValues { get; set; }
       

        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            modelBuilder.Entity<CryptoCurrency>()
                .HasMany(c => c.Values)
                .WithOne(v => v.CryptoCurrency)
                .HasForeignKey(v => v.CryptoCurrencyId);

            modelBuilder.Entity<CryptoCurrencyValue>()
                .Property(v => v.Price)
                .HasColumnType("decimal(18, 8)");

            modelBuilder.Entity<CryptoCurrencyValue>()
                .Property(v => v.Change24h)
                .HasColumnType("decimal(18, 8)");
        }
    }
}
