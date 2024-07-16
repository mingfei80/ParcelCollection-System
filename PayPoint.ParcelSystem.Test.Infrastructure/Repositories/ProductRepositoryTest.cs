using Moq;
using PayPoint.ParcelSystem.Domain.Models;
using PayPoint.ParcelSystem.Infrastructure.Data;
using PayPoint.ParcelSystem.Infrastructure.Repositories;

namespace PayPoint.ParcelSystem.Test.Infrastructure.Repositories;

public class ProductRepositoryTest
{
    [Fact]
    public async Task GetProductByIdAsync_ShouldReturnProduct_WhenProductIdExists()
    {
        // Arrange
        var productId = 1;
        var expectedProduct = new Product { ProductId = productId, Name = "Test Product", SupplierId = 1 };

        var mockDbContext = new Mock<IDbContext>();
        mockDbContext.Setup(db => db.Products).Returns(new List<Product>
                                                            {
                                                                expectedProduct
                                                            }.AsQueryable());

        var productRepository = new ProductRepository(mockDbContext.Object);

        // Act
        var result = await productRepository.GetProductByIdAsync(productId);

        // Assert
        Assert.NotNull(result);
        Assert.Equal(expectedProduct.ProductId, result.ProductId);
        Assert.Equal(expectedProduct.Name, result.Name);
    }


    [Fact]
    public async Task GetProductsByIdsAsync_ShouldReturnProducts_WhenProductIdsExist()
    {
        // Arrange
        var productIds = new List<int> { 1, 2 };
        var expectedProducts = new List<Product>
    {
        new Product { ProductId = 1, Name = "Product 1", SupplierId = 1 },
        new Product { ProductId = 2, Name = "Product 2", SupplierId = 1 }
    };

        var mockDbContext = new Mock<IDbContext>();
        mockDbContext.Setup(db => db.Products).Returns(expectedProducts.AsQueryable());

        var productRepository = new ProductRepository(mockDbContext.Object);

        // Act
        var result = await productRepository.GetProductsByIdsAsync(productIds);

        // Assert
        Assert.NotNull(result);
        Assert.Equal(expectedProducts.Count, result.Count);
        Assert.Contains(result, p => p.ProductId == 1);
        Assert.Contains(result, p => p.ProductId == 2);
    }

    [Fact]
    public async Task GetProductsByIdsAsync_ShouldReturnEmptyList_WhenProductIdsDoNotExist()
    {
        // Arrange
        var productIds = new List<int> { 1, 2 };

        var mockDbContext = new Mock<IDbContext>();
        mockDbContext.Setup(db => db.Products).Returns(new List<Product>
                                                {
                                                    new Product { ProductId = 3, Name = "Product 3", SupplierId = 1 }
                                                }.AsQueryable());

        var productRepository = new ProductRepository(mockDbContext.Object);

        // Act
        var result = await productRepository.GetProductsByIdsAsync(productIds);

        // Assert
        Assert.NotNull(result);
        Assert.Empty(result);
    }
}
