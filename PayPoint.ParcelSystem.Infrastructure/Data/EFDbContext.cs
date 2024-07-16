using Microsoft.EntityFrameworkCore;
using PayPoint.ParcelSystem.Domain.Models;

namespace PayPoint.ParcelSystem.Infrastructure.Data;

public class EFDbContext : DbContext, IDbContext
{
    public DbSet<Product> Products { get; set; }
    public DbSet<Supplier> Suppliers { get; set; }

    IQueryable<Product> IDbContext.Products
    {
        get
        {
            return Products.AsQueryable();
        }
    }

    IQueryable<Supplier> IDbContext.Suppliers
    {
        get
        {
            return Suppliers.AsQueryable();
        }
    }

    public EFDbContext(DbContextOptions<EFDbContext> options) : base(options) { }
}
