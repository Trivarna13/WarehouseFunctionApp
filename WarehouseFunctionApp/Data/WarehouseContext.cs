using Microsoft.EntityFrameworkCore;
using WarehouseFunctionApp.Models;

namespace WarehouseFunctionApp.Data
{
    public class WarehouseContext : DbContext
    {
        public DbSet<Warehouse> Warehouses { get; set; }

        public WarehouseContext(DbContextOptions<WarehouseContext> options) : base(options) { }

        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            modelBuilder.Entity<Warehouse>()
                .Property(w => w.Id)
                .ValueGeneratedOnAdd();
        }
    }

}
