using PayPoint.ParcelSystem.Domain.Interfaces;
using PayPoint.ParcelSystem.Domain.Models;

namespace PayPoint.ParcelSystem.Domain.Services;

public class CollectionService : ICollectionService
{
    private readonly IProductRepository _productRepository;
    private readonly ISupplierRepository _supplierRepository;

    public CollectionService(IProductRepository productRepository, ISupplierRepository supplierRepository)
    {
        _productRepository = productRepository;
        _supplierRepository = supplierRepository;
    }

    public async Task<CollectionDate> CalculateCollectionDateAsync(List<int> productIds, DateTime orderDate)
    {
        // Fetch all products at once
        var products = await _productRepository.GetProductsByIdsAsync(productIds);

        // Get unique supplier IDs from the products
        var supplierIds = products.Select(p => p.SupplierId).Distinct().ToList();

        // Fetch all suppliers at once
        var suppliers = await _supplierRepository.GetSuppliersByIdsAsync(supplierIds);

        // Calculate the maximum lead time
        var maxLeadDays = products
            .Select(p => suppliers.Single(s => s.SupplierId == p.SupplierId).LeadTime)
            .Max();

        return new CollectionDate { Date = LeadDateIncludedWeekends(orderDate, maxLeadDays) };
    }

    private DateTime LeadDateIncludedWeekends(DateTime startDate, int leadDays)
    {
        var currentDate = startDate;
        var remamingLeadDays = leadDays;

        if (leadDays < 1 && IsBusinessDay(currentDate)) // We 
            return startDate;

        while (remamingLeadDays > 0 || !IsBusinessDay(currentDate))
        {
            if (IsBusinessDay(currentDate))
            {
                remamingLeadDays--;
            }

            currentDate = currentDate.AddDays(1);
        }

        return currentDate;
    }

    private bool IsBusinessDay(DateTime currentDate)
    {
        return currentDate.DayOfWeek != DayOfWeek.Saturday && currentDate.DayOfWeek != DayOfWeek.Sunday;
    }
}
