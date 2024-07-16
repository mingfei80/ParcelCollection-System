using PayPoint.ParcelSystem.Domain.Models;

namespace PayPoint.ParcelSystem.Domain.Interfaces;

public interface ISupplierRepository
{
    Task<Supplier> GetSupplierByIdAsync(int supplierId);
    Task<List<Supplier>> GetSuppliersByIdsAsync(List<int> supplierIds);
}