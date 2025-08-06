using InventoryManagementAPI.Data;
using InventoryManagementAPI.Models;
using Microsoft.EntityFrameworkCore;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace InventoryManagementAPI.Services
{
    public class ProductService
    {
        private readonly InventoryDbContext _context;
        public ProductService(InventoryDbContext context)
        {
            _context = context;
        }

        public virtual async Task<List<Product>> GetAllAsync()
        {
            return await _context.Products.ToListAsync();
        }

        public virtual async Task<Product?> GetByIdAsync(int id)
        {
            return await _context.Products.FindAsync(id);
        }

        public virtual async Task<Product> AddAsync(Product product)
        {
            _context.Products.Add(product);
            await _context.SaveChangesAsync();
            return product;
        }

        public virtual async Task<bool> UpdateAsync(int id, Product product)
        {
            var existing = await _context.Products.FindAsync(id);
            if (existing == null) return false;
            existing.Name = product.Name;
            existing.Description = product.Description;
            existing.Price = product.Price;
            await _context.SaveChangesAsync();
            return true;
        }

        public virtual async Task<bool> DeleteAsync(int id)
        {
            var existing = await _context.Products.FindAsync(id);
            if (existing == null) return false;
            _context.Products.Remove(existing);
            await _context.SaveChangesAsync();
            return true;
        }
    }
}