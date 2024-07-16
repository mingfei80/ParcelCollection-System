using PayPoint.ParcelSystem.Domain.Interfaces;
using PayPoint.ParcelSystem.Domain.Models;
using PayPoint.ParcelSystem.Infrastructure.Data;

namespace PayPoint.ParcelSystem.Infrastructure.Repositories;

public class SupplierRepository : ISupplierRepository
{
    private readonly IDbContext _dbContext;

    public SupplierRepository(IDbContext dbContext)
    {
        _dbContext = dbContext;
    }

    public Task<Supplier> GetSupplierByIdAsync(int supplierId)
    {
        return Task.FromResult(_dbContext.Suppliers.Single(x => x.SupplierId == supplierId));
    }

    public Task<List<Supplier>> GetSuppliersByIdsAsync(List<int> supplierIds)
    {
        return Task.FromResult(_dbContext.Suppliers.Where(p => supplierIds.Contains(p.SupplierId)).ToList());
    }
}
