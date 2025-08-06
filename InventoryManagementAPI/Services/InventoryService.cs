using InventoryManagementAPI.Data;
using InventoryManagementAPI.Models;
using Microsoft.EntityFrameworkCore;

namespace InventoryManagementAPI.Services
{
    public class InventoryService
    {
        private readonly InventoryDbContext _context;
        public InventoryService(InventoryDbContext context)
        {
            _context = context;
        }

        public virtual async Task<List<Inventory>> GetAllAsync()
        {
            return await _context.Inventories.ToListAsync();
        }

        public virtual async Task<Inventory?> GetByIdAsync(int id)
        {
            return await _context.Inventories.FindAsync(id);
        }

        public virtual async Task<Inventory> AddAsync(Inventory inventory)
        {
            if (!_context.Products.Any(p => p.Id == inventory.ProductId))
                throw new ArgumentException("Invalid ProductId");
            if (!_context.Suppliers.Any(s => s.Id == inventory.SupplierId))
                throw new ArgumentException("Invalid SupplierId");
            if (!_context.Warehouses.Any(w => w.Id == inventory.WarehouseId))
                throw new ArgumentException("Invalid WarehouseId");
            _context.Inventories.Add(inventory);
            await _context.SaveChangesAsync();
            return inventory;
        }

        public virtual async Task<bool> UpdateAsync(int id, Inventory inventory)
        {
            var existing = await _context.Inventories.FindAsync(id);
            if (existing == null) return false;
            if (!_context.Products.Any(p => p.Id == inventory.ProductId))
                throw new ArgumentException("Invalid ProductId");
            if (!_context.Suppliers.Any(s => s.Id == inventory.SupplierId))
                throw new ArgumentException("Invalid SupplierId");
            if (!_context.Warehouses.Any(w => w.Id == inventory.WarehouseId))
                throw new ArgumentException("Invalid WarehouseId");
            existing.ProductId = inventory.ProductId;
            existing.SupplierId = inventory.SupplierId;
            existing.WarehouseId = inventory.WarehouseId;
            existing.Quantity = inventory.Quantity;
            existing.Name = inventory.Name;
            existing.Description = inventory.Description;
            await _context.SaveChangesAsync();
            return true;
        }

        public virtual async Task<bool> DeleteAsync(int id)
        {
            var existing = await _context.Inventories.FindAsync(id);
            if (existing == null) return false;
            _context.Inventories.Remove(existing);
            await _context.SaveChangesAsync();
            return true;
        }
    }
}