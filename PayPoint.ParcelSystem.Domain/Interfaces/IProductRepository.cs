using PayPoint.ParcelSystem.Domain.Models;

namespace PayPoint.ParcelSystem.Domain.Interfaces;

public interface IProductRepository
{
    Task<Product> GetProductByIdAsync(int productId);
    Task<List<Product>> GetProductsByIdsAsync(List<int> productIds);
}