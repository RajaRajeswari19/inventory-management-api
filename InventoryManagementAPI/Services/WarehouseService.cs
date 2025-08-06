using InventoryManagementAPI.Data;
using InventoryManagementAPI.Models;
using Microsoft.EntityFrameworkCore;

namespace InventoryManagementAPI.Services
{
    public class WarehouseService
    {
        private readonly InventoryDbContext _context;
        public WarehouseService(InventoryDbContext context)
        {
            _context = context;
        }

        public virtual async Task<List<Warehouse>> GetAllAsync()
        {
            return await _context.Warehouses.ToListAsync();
        }

        public virtual async Task<Warehouse?> GetByIdAsync(int id)
        {
            return await _context.Warehouses.FindAsync(id);
        }

        public virtual async Task<Warehouse> AddAsync(Warehouse warehouse)
        {
            _context.Warehouses.Add(warehouse);
            await _context.SaveChangesAsync();
            return warehouse;
        }

        public virtual async Task<bool> UpdateAsync(int id, Warehouse warehouse)
        {
            var existing = await _context.Warehouses.FindAsync(id);
            if (existing == null) return false;
            existing.Name = warehouse.Name;
            existing.Location = warehouse.Location;
            existing.Capacity = warehouse.Capacity;
            await _context.SaveChangesAsync();
            return true;
        }

        public virtual async Task<bool> DeleteAsync(int id)
        {
            var existing = await _context.Warehouses.FindAsync(id);
            if (existing == null) return false;
            _context.Warehouses.Remove(existing);
            await _context.SaveChangesAsync();
            return true;
        }
    }
}