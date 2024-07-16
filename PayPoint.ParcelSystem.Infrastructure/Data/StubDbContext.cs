using PayPoint.ParcelSystem.Domain.Models;
using PayPoint.ParcelSystem.Infrastructure.TestData;

namespace PayPoint.ParcelSystem.Infrastructure.Data;

public class StubDbContext : IDbContext
{
    public IQueryable<Supplier> Suppliers
    {
        get
        {
            return StubTestData.Suppliers().AsQueryable();
        }
    }

    public IQueryable<Product> Products
    {
        get
        {
            return StubTestData.Products().AsQueryable();
        }
    }
}
