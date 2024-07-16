using PayPoint.ParcelSystem.Domain.Models;

namespace PayPoint.ParcelSystem.Domain.Interfaces
{
    public interface ICollectionService
    {
        Task<CollectionDate> CalculateCollectionDateAsync(List<int> productIds, DateTime orderDate);
    }
}