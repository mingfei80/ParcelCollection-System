using PayPoint.ParcelSystem.Domain.Services;
using PayPoint.ParcelSystem.Infrastructure.Repositories;
using ParcelSystemDBContext = PayPoint.ParcelSystem.Infrastructure.Data.StubDbContext;

namespace PayPoint.ParcelSystem.Test.Infrastructure.TestData;

public class DataIntegrationTests
{
    private readonly CollectionService _collectionService;
    private readonly ParcelSystemDBContext _context;

    public DataIntegrationTests()
    {
        _context = new ParcelSystemDBContext();

        var productRepository = new ProductRepository(_context);
        var supplierRepository = new SupplierRepository(_context);

        _collectionService = new CollectionService(productRepository, supplierRepository);
    }

    #region TestData
    public static IEnumerable<object[]> LeadTimeOfOneDayTestData()
    {
        //The order on a Friday - 05/01/2018, The collection date is Monday - 08/01/2018
        yield return new object[] { new DateTime(2018, 1, 5), new DateTime(2018, 1, 8) };

        //The order on a Saturday - 06/01/2018, The collection date is Tuesday - 09/01/2018
        yield return new object[] { new DateTime(2018, 1, 6), new DateTime(2018, 1, 9) };

        //The order on a Sunday - 07/01/2018, The collection date is Tuesday - 09/01/2018
        yield return new object[] { new DateTime(2018, 1, 7), new DateTime(2018, 1, 9) };

        //The order on a Monday - 08/01/2018, The collection date is Tuesday - 09/01/2018
        yield return new object[] { new DateTime(2018, 1, 8), new DateTime(2018, 1, 9) };
    }

    public static IEnumerable<object[]> LeadTimeOfTwoDayTestData()
    {
        //The order on a Thursday - 04/01/2018, The collection date is Monday - 08/01/2018
        yield return new object[] { new DateTime(2018, 1, 4), new DateTime(2018, 1, 8) };

        //The order on a Friday - 05/01/2018, The collection date is Tuesday - 09/01/2018
        yield return new object[] { new DateTime(2018, 1, 5), new DateTime(2018, 1, 9) };

        //The order on a Saturday - 06/01/2018, The collection date is Wednesday - 10/01/2018
        yield return new object[] { new DateTime(2018, 1, 6), new DateTime(2018, 1, 10) };

        //The order on a Sunday - 08/01/2018, The collection date is Wednesday - 10/01/2018
        yield return new object[] { new DateTime(2018, 1, 7), new DateTime(2018, 1, 10) };
    }

    public static IEnumerable<object[]> LeadTimeOfThreeDayTestData()
    {
        //The order on a Thursday - 04/01/2018, The collection date is Tuesday - 09/01/2018
        yield return new object[] { new DateTime(2018, 1, 4), new DateTime(2018, 1, 9) };

        //The order on a Friday - 05/01/2018, The collection date is Wednesday - 09/01/2018
        yield return new object[] { new DateTime(2018, 1, 5), new DateTime(2018, 1, 10) };

        //The order on a Saturday - 06/01/2018, The collection date is Thursday - 11/01/2018
        yield return new object[] { new DateTime(2018, 1, 6), new DateTime(2018, 1, 11) };

        //The order on a Sunday - 08/01/2018, The collection date is Thursday - 11/01/2018
        yield return new object[] { new DateTime(2018, 1, 7), new DateTime(2018, 1, 11) };
    }
    #endregion

    [Theory]
    [MemberData(nameof(LeadTimeOfOneDayTestData))]
    public async Task OneProductWithLeadTimeOfOneDay(DateTime orderDate, DateTime expectedCollectionDate)
    {
        var result = await _collectionService.CalculateCollectionDateAsync(new List<int>() { 1 }, orderDate);
        Assert.Equal(expectedCollectionDate, result.Date.Date);
    }

    [Theory]
    [MemberData(nameof(LeadTimeOfTwoDayTestData))]
    public async Task OneProductWithLeadTimeOfTwoDay(DateTime orderDate, DateTime expectedCollectionDate)
    {
        var result = await _collectionService.CalculateCollectionDateAsync(new List<int>() { 2 }, orderDate);
        Assert.Equal(expectedCollectionDate, result.Date.Date);
    }

    [Theory]
    [MemberData(nameof(LeadTimeOfThreeDayTestData))]
    public async Task OneProductWithLeadTimeOfThreeDay(DateTime orderDate, DateTime expectedCollectionDate)
    {
        var result = await _collectionService.CalculateCollectionDateAsync(new List<int>() { 3 }, orderDate);
        Assert.Equal(expectedCollectionDate, result.Date.Date);
    }

    [Fact]
    public async Task SaturdayHasExtraTwoDays()
    {
        var result = await _collectionService.CalculateCollectionDateAsync(new List<int>() { 1 }, new DateTime(2018, 1, 26));
        Assert.Equal(new DateTime(2018, 1, 26).Date.AddDays(3), result.Date.Date);
    }

    [Fact]
    public async Task SundayHasExtraDay()
    {
        var result = await _collectionService.CalculateCollectionDateAsync(new List<int>() { 3 }, new DateTime(2018, 1, 25));
        Assert.Equal(new DateTime(2018, 1, 25).Date.AddDays(5), result.Date.Date);
    }
}
