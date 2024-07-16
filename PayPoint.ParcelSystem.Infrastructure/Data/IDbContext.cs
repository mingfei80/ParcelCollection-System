using PayPoint.ParcelSystem.Domain.Models;

namespace PayPoint.ParcelSystem.Infrastructure.Data;

public interface IDbContext
{
    IQueryable<Supplier> Suppliers { get; }

    IQueryable<Product> Products { get; }
}
