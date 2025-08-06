using InventoryManagementAPI.Data;
using InventoryManagementAPI.Models;
using Microsoft.EntityFrameworkCore;

namespace InventoryManagementAPI.Services
{
    public class SupplierService
    {
        private readonly InventoryDbContext _context;
        public SupplierService(InventoryDbContext context)
        {
            _context = context;
        }

        public virtual async Task<List<Supplier>> GetAllAsync()
        {
            return await _context.Suppliers.ToListAsync();
        }

        public virtual async Task<Supplier?> GetByIdAsync(int id)
        {
            return await _context.Suppliers.FindAsync(id);
        }

        public virtual async Task<Supplier> AddAsync(Supplier supplier)
        {
            _context.Suppliers.Add(supplier);
            await _context.SaveChangesAsync();
            return supplier;
        }

        public virtual async Task<bool> UpdateAsync(int id, Supplier supplier)
        {
            var existing = await _context.Suppliers.FindAsync(id);
            if (existing == null) return false;
            existing.Name = supplier.Name;
            existing.Contact = supplier.Contact;
            existing.Address = supplier.Address;
            await _context.SaveChangesAsync();
            return true;
        }

        public virtual async Task<bool> DeleteAsync(int id)
        {
            var existing = await _context.Suppliers.FindAsync(id);
            if (existing == null) return false;
            _context.Suppliers.Remove(existing);
            await _context.SaveChangesAsync();
            return true;
        }
    }
}