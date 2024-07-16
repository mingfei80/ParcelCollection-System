using PayPoint.ParcelSystem.Domain.Interfaces;
using PayPoint.ParcelSystem.Domain.Models;
using PayPoint.ParcelSystem.Infrastructure.Data;

namespace PayPoint.ParcelSystem.Infrastructure.Repositories;

public class ProductRepository : IProductRepository
{
    private readonly IDbContext _dbContext;

    public ProductRepository(IDbContext dbContext)
    {
        _dbContext = dbContext;
    }

    public Task<Product> GetProductByIdAsync(int productId)
    {
        return Task.FromResult(_dbContext.Products.Single(x => x.ProductId == productId));
    }

    public Task<List<Product>> GetProductsByIdsAsync(List<int> productIds)
    {
        return Task.FromResult(_dbContext.Products.Where(p => productIds.Contains(p.ProductId)).ToList());
    }
}
