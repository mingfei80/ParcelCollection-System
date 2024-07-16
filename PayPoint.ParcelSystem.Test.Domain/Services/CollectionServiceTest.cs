using Moq;
using PayPoint.ParcelSystem.Domain.Interfaces;
using PayPoint.ParcelSystem.Domain.Models;
using PayPoint.ParcelSystem.Domain.Services;

namespace PayPoint.ParcelSystem.Test.Domain.Services;

public class CollectionServiceTests
{
    private readonly Mock<IProductRepository> _productRepository;
    private readonly Mock<ISupplierRepository> _supplierRepository;
    private readonly ICollectionService _collectionService;

    public CollectionServiceTests()
    {
        _productRepository = new Mock<IProductRepository>();
        _supplierRepository = new Mock<ISupplierRepository>();

        _collectionService = new CollectionService(_productRepository.Object, _supplierRepository.Object);
    }

    #region TestData
    public static IEnumerable<object[]> ZeroLeadTimeAddedToCollectionDateTestData()
    {
        //The order on a Friday - 05/01/2018, The collection date is Friday - 05/01/2018
        yield return new object[] { new DateTime(2018, 1, 5), 0, new DateTime(2018, 1, 5) };

        //The order on a Saturday - 06/01/2018, The collection date is Monday - 08/01/2018
        yield return new object[] { new DateTime(2018, 1, 6), 0, new DateTime(2018, 1, 8) };

        //The order on a Sunday - 07/01/2018, The collection date is Monday - 08/01/2018
        yield return new object[] { new DateTime(2018, 1, 7), 0, new DateTime(2018, 1, 8) };

        //The order on a Monday - 08/01/2018, The collection date is Monday - 08/01/2018
        yield return new object[] { new DateTime(2018, 1, 8), 0, new DateTime(2018, 1, 8) };
    }

    public static IEnumerable<object[]> LeadTimeAddedToCollectionDateTestData()
    {
        //The order on a Monday - 01/01/2018, The collection date is Tuesday - 02/01/2018
        yield return new object[] { new DateTime(2018, 1, 1), 1, new DateTime(2018, 1, 2) };

        //The order on a Monday - 01/01/2018, The collection date is Wednesday - 03/01/2018
        yield return new object[] { new DateTime(2018, 1, 1), 2, new DateTime(2018, 1, 3) };
    }

    public static IEnumerable<object[]> LeadTimeIsNotCountedOverAWeekendTestData()
    {
        //The order on a Friday - 05/01/2018, The collection date is Monday - 08/01/2018
        yield return new object[] { new DateTime(2018, 1, 5), 1, new DateTime(2018, 1, 8) };

        ////The order on a Saturday - 06/01/2018, The collection date is Tuesday - 09/01/2018
        yield return new object[] { new DateTime(2018, 1, 6), 1, new DateTime(2018, 1, 9) };

        ////The order on a Sunday - 07/01/2018, The collection date is Tuesday - 09/01/2018
        yield return new object[] { new DateTime(2018, 1, 7), 1, new DateTime(2018, 1, 9) };
    }

    public static IEnumerable<object[]> LeadTimeOverMultipleWeeksTestData()
    {
        //The order on a Friday - 05/01/2018, The collection date is Monday - 15/01/2018
        yield return new object[] { new DateTime(2018, 1, 5), 6, new DateTime(2018, 1, 15) };

        //The order on a Friday - 05/01/2018, The collection date is Monday - 22/01/2018
        yield return new object[] { new DateTime(2018, 1, 5), 11, new DateTime(2018, 1, 22) };
    }
    #endregion

    [Theory]
    [MemberData(nameof(ZeroLeadTimeAddedToCollectionDateTestData))]
    [MemberData(nameof(LeadTimeAddedToCollectionDateTestData))]
    [MemberData(nameof(LeadTimeIsNotCountedOverAWeekendTestData))]
    [MemberData(nameof(LeadTimeOverMultipleWeeksTestData))]
    public async Task ShouldWorkCalculateCollectionDate(DateTime orderDate, int leadTime, DateTime expectedCollectionDate)
    {
        var supplierId = 1;
        var productIds = new List<int> { 1 };

        _productRepository.Setup(repo => repo.GetProductsByIdsAsync(It.IsAny<List<int>>()))
            .ReturnsAsync(new List<Product> { new Product { ProductId = productIds[0], SupplierId = supplierId } });

        _supplierRepository.Setup(repo => repo.GetSuppliersByIdsAsync(It.IsAny<List<int>>()))
            .ReturnsAsync(new List<Supplier> { new Supplier { SupplierId = supplierId, LeadTime = leadTime } });

        // Act
        var result = await _collectionService.CalculateCollectionDateAsync(productIds, orderDate);

        // Assert
        Assert.Equal(expectedCollectionDate, result.Date);
    }


    //Supplier with longest lead time is used for calculation
    [Fact]
    public async Task ShouldWorkMultipleSuppliersCalculateCollectionDate()
    {
        // Arrange
        var supplierId01 = 1;
        var supplierId02 = 2;
        var productIds = new List<int> { 1, 2 };
        var orderDate = new DateTime(2018, 1, 1);
        var expectedCollectionDate = new DateTime(2018, 1, 3);

        _productRepository.Setup(repo => repo.GetProductsByIdsAsync(It.IsAny<List<int>>()))
            .ReturnsAsync(new List<Product> {
                        new Product { ProductId = productIds[0], SupplierId = supplierId01 },
                        new Product { ProductId = productIds[1], SupplierId = supplierId02 }
                        });

        _supplierRepository.Setup(repo => repo.GetSuppliersByIdsAsync(It.IsAny<List<int>>()))
            .ReturnsAsync(new List<Supplier> {
                        new Supplier { SupplierId = supplierId01, LeadTime = 1 },
                        new Supplier { SupplierId = supplierId02, LeadTime = 2 },
                        });

        // Act
        var result = await _collectionService.CalculateCollectionDateAsync(productIds, orderDate);

        // Assert
        Assert.Equal(expectedCollectionDate, result.Date);
    }
}
