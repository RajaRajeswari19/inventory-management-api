using Microsoft.EntityFrameworkCore;
using InventoryManagementAPI.Models;

namespace InventoryManagementAPI.Data
{
    public class InventoryDbContext : DbContext
    {
        public InventoryDbContext(DbContextOptions<InventoryDbContext> options) : base(options) { }

        public DbSet<Inventory> Inventories { get; set; }
        public DbSet<Product> Products { get; set; }
        public DbSet<Supplier> Suppliers { get; set; }
        public DbSet<Warehouse> Warehouses { get; set; }

        public static void SeedInitialData(InventoryDbContext context)
        {
            // Create Products first
            if (!context.Products.Any())
            {
                context.Products.AddRange(
                    new Product { Name = "Laptop", Description = "Dell XPS 13", Price = 1200 },
                    new Product { Name = "Monitor", Description = "24 inch LED", Price = 200 },
                    new Product { Name = "Keyboard", Description = "Mechanical", Price = 80 }
                );
                context.SaveChanges();
            }

            // Create Suppliers
            if (!context.Suppliers.Any())
            {
                context.Suppliers.AddRange(
                    new Supplier { Name = "ABC Supplies", Contact = "123-456-7890", Address = "123 Main St" },
                    new Supplier { Name = "XYZ Traders", Contact = "987-654-3210", Address = "456 Market Ave" }
                );
                context.SaveChanges();
            }

            // Create Warehouses
            if (!context.Warehouses.Any())
            {
                context.Warehouses.AddRange(
                    new Warehouse { Name = "Central Warehouse", Location = "Downtown", Capacity = 1000 },
                    new Warehouse { Name = "East Warehouse", Location = "Eastside", Capacity = 500 }
                );
                context.SaveChanges();
            }

            // Create Inventories with proper foreign key relationships
            if (!context.Inventories.Any())
            {
                var products = context.Products.ToList();
                var suppliers = context.Suppliers.ToList();
                var warehouses = context.Warehouses.ToList();

                if (products.Any() && suppliers.Any() && warehouses.Any())
                {
                    context.Inventories.AddRange(
                        new Inventory { 
                            Name = "Laptop", 
                            Quantity = 10, 
                            Description = "Dell XPS 13",
                            ProductId = products[0].Id,
                            SupplierId = suppliers[0].Id,
                            WarehouseId = warehouses[0].Id
                        },
                        new Inventory { 
                            Name = "Monitor", 
                            Quantity = 15, 
                            Description = "24 inch LED",
                            ProductId = products[1].Id,
                            SupplierId = suppliers[0].Id,
                            WarehouseId = warehouses[0].Id
                        },
                        new Inventory { 
                            Name = "Keyboard", 
                            Quantity = 30, 
                            Description = "Mechanical",
                            ProductId = products[2].Id,
                            SupplierId = suppliers[1].Id,
                            WarehouseId = warehouses[1].Id
                        }
                    );
                    context.SaveChanges();
                }
            }
        }
    }
}