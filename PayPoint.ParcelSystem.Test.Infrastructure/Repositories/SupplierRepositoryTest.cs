using Moq;
using PayPoint.ParcelSystem.Domain.Models;
using PayPoint.ParcelSystem.Infrastructure.Data;
using PayPoint.ParcelSystem.Infrastructure.Repositories;

namespace PayPoint.ParcelSystem.Test.Infrastructure.Repositories;

public class SupplierRepositoryTest
{
    [Fact]
    public async Task GetSupplierByIdAsync_ShouldReturnSupplier_WhenSupplierIdExists()
    {
        // Arrange
        var supplierId = 1;
        var expectedSupplier = new Supplier { SupplierId = supplierId, Name = "Test Supplier" };

        var mockDbContext = new Mock<IDbContext>();
        mockDbContext.Setup(db => db.Suppliers).Returns(new List<Supplier>
                                                            {
                                                                expectedSupplier
                                                            }.AsQueryable());

        var supplierRepository = new SupplierRepository(mockDbContext.Object);

        // Act
        var result = await supplierRepository.GetSupplierByIdAsync(supplierId);

        // Assert
        Assert.NotNull(result);
        Assert.Equal(expectedSupplier.SupplierId, result.SupplierId);
        Assert.Equal(expectedSupplier.Name, result.Name);
    }


    [Fact]
    public async Task GetSuppliersByIdsAsync_ShouldReturnSuppliers_WhenSupplierIdsExist()
    {
        // Arrange
        var supplierIds = new List<int> { 1, 2 };
        var expectedSuppliers = new List<Supplier>
                                        {
                                            new Supplier { SupplierId = 1, Name = "Supplier 1" },
                                            new Supplier { SupplierId = 2, Name = "Supplier 2" }
                                        };

        var mockDbContext = new Mock<IDbContext>();
        mockDbContext.Setup(db => db.Suppliers).Returns(expectedSuppliers.AsQueryable());

        var supplierRepository = new SupplierRepository(mockDbContext.Object);

        // Act
        var result = await supplierRepository.GetSuppliersByIdsAsync(supplierIds);

        // Assert
        Assert.NotNull(result);
        Assert.Equal(expectedSuppliers.Count, result.Count);
        Assert.Contains(result, p => p.SupplierId == 1);
        Assert.Contains(result, p => p.SupplierId == 2);
    }

    [Fact]
    public async Task GetSuppliersByIdsAsync_ShouldReturnEmptyList_WhenSupplierIdsDoNotExist()
    {
        // Arrange
        var supplierIds = new List<int> { 1, 2 };

        var mockDbContext = new Mock<IDbContext>();
        mockDbContext.Setup(db => db.Suppliers).Returns(new List<Supplier>
                                                {
                                                    new Supplier { SupplierId = 3, Name = "Supplier 3" }
                                                }.AsQueryable());

        var supplierRepository = new SupplierRepository(mockDbContext.Object);

        // Act
        var result = await supplierRepository.GetSuppliersByIdsAsync(supplierIds);

        // Assert
        Assert.NotNull(result);
        Assert.Empty(result);
    }
}
