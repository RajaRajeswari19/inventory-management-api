using InventoryManagementAPI.Data;
using InventoryManagementAPI.Models;
using InventoryManagementAPI.Services;
using Microsoft.EntityFrameworkCore;
using Moq;
using Xunit;
using System.Threading.Tasks;
using System.Collections.Generic;
using System.Linq;

namespace InventoryManagementAPI.Tests
{
    public class InventoryServiceTests
    {
        private InventoryDbContext GetDbContext(string dbName)
        {
            var options = new DbContextOptionsBuilder<InventoryDbContext>()
                .UseInMemoryDatabase(databaseName: dbName)
                .Options;
            return new InventoryDbContext(options);
        }

        [Fact]
        public async Task AddAsync_ShouldAddInventory()
        {
            var context = GetDbContext("AddInventory");
            // Add required related entities
            var product = new Product { Name = "TestProduct", Price = 10 };
            var supplier = new Supplier { Name = "TestSupplier" };
            var warehouse = new Warehouse { Name = "TestWarehouse", Capacity = 100 };
            context.Products.Add(product);
            context.Suppliers.Add(supplier);
            context.Warehouses.Add(warehouse);
            context.SaveChanges();
            var service = new InventoryService(context);
            var inventory = new Inventory {
                Name = "Item1",
                Quantity = 10,
                ProductId = product.Id,
                SupplierId = supplier.Id,
                WarehouseId = warehouse.Id
            };
            var result = await service.AddAsync(inventory);
            Assert.NotNull(result);
            Assert.Equal("Item1", result.Name);
            Assert.Equal(10, result.Quantity);
        }

        [Fact]
        public async Task GetAllAsync_ShouldReturnAllInventories()
        {
            var context = GetDbContext("GetAllInventories");
            context.Inventories.Add(new Inventory { Name = "Item1", Quantity = 5 });
            context.Inventories.Add(new Inventory { Name = "Item2", Quantity = 15 });
            context.SaveChanges();
            var service = new InventoryService(context);

            var result = await service.GetAllAsync();

            Assert.Equal(2, result.Count);
        }

        [Fact]
        public async Task GetByIdAsync_ShouldReturnInventory_WhenExists()
        {
            var context = GetDbContext("GetById");
            var inventory = new Inventory { Name = "Item1", Quantity = 5 };
            context.Inventories.Add(inventory);
            context.SaveChanges();
            var service = new InventoryService(context);

            var result = await service.GetByIdAsync(inventory.Id);

            Assert.NotNull(result);
            Assert.Equal("Item1", result.Name);
        }

        [Fact]
        public async Task GetByIdAsync_ShouldReturnNull_WhenNotExists()
        {
            var context = GetDbContext("GetByIdNull");
            var service = new InventoryService(context);

            var result = await service.GetByIdAsync(999);

            Assert.Null(result);
        }

        [Fact]
        public async Task UpdateAsync_ShouldUpdateInventory_WhenExists()
        {
            var context = GetDbContext("UpdateInventory");
            // Add required related entities
            var product = new Product { Name = "TestProduct", Price = 10 };
            var supplier = new Supplier { Name = "TestSupplier" };
            var warehouse = new Warehouse { Name = "TestWarehouse", Capacity = 100 };
            context.Products.Add(product);
            context.Suppliers.Add(supplier);
            context.Warehouses.Add(warehouse);
            context.SaveChanges();
            var inventory = new Inventory {
                Name = "Item1",
                Quantity = 5,
                ProductId = product.Id,
                SupplierId = supplier.Id,
                WarehouseId = warehouse.Id
            };
            context.Inventories.Add(inventory);
            context.SaveChanges();
            var service = new InventoryService(context);
            var updated = new Inventory {
                Name = "Updated",
                Quantity = 20,
                ProductId = product.Id,
                SupplierId = supplier.Id,
                WarehouseId = warehouse.Id
            };
            var result = await service.UpdateAsync(inventory.Id, updated);
            Assert.True(result);
            var dbItem = context.Inventories.First();
            Assert.Equal("Updated", dbItem.Name);
            Assert.Equal(20, dbItem.Quantity);
        }

        [Fact]
        public async Task UpdateAsync_ShouldReturnFalse_WhenNotExists()
        {
            var context = GetDbContext("UpdateNotExists");
            var service = new InventoryService(context);
            var updated = new Inventory { Name = "Updated", Quantity = 20 };

            var result = await service.UpdateAsync(999, updated);

            Assert.False(result);
        }

        [Fact]
        public async Task DeleteAsync_ShouldDeleteInventory_WhenExists()
        {
            var context = GetDbContext("DeleteInventory");
            var inventory = new Inventory { Name = "Item1", Quantity = 5 };
            context.Inventories.Add(inventory);
            context.SaveChanges();
            var service = new InventoryService(context);

            var result = await service.DeleteAsync(inventory.Id);

            Assert.True(result);
            Assert.Empty(context.Inventories);
        }

        [Fact]
        public async Task DeleteAsync_ShouldReturnFalse_WhenNotExists()
        {
            var context = GetDbContext("DeleteNotExists");
            var service = new InventoryService(context);

            var result = await service.DeleteAsync(999);

            Assert.False(result);
        }
    }
}