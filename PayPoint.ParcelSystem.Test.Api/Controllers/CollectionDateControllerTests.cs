using AutoMapper;
using Microsoft.AspNetCore.Mvc;
using Moq;
using PayPoint.ParcelSystem.Api.Controllers;
using PayPoint.ParcelSystem.Api.Dtos.CollectionsDate;
using PayPoint.ParcelSystem.Domain.Interfaces;
using PayPoint.ParcelSystem.Domain.Models;

namespace PayPoint.ParcelSystem.Test.Api.Controllers;

public class CollectionDateControllerTests
{
    [Fact]
    public async Task Get_ShouldReturnBadRequest_WhenProductIdIsNull()
    {
        // Arrange
        List<int> productIds = new List<int>();
        var orderDate = new DateTime(2024, 7, 7);

        var mockCollectionService = new Mock<ICollectionService>();
        mockCollectionService
            .Setup(service => service.CalculateCollectionDateAsync(productIds, orderDate))
            .ThrowsAsync(new Exception("Test exception"));

        var mockMapper = new Mock<IMapper>();

        var mockLogger = new Mock<ILogger>();

        var controller = new CollectionDateController(mockMapper.Object, mockCollectionService.Object, mockLogger.Object);

        // Act
        var result = await controller.Get(productIds, orderDate);

        // Assert
        Assert.IsType<BadRequestObjectResult>(result.Result);
    }

    [Fact]
    public async Task Get_ShouldReturnCollectionResultDto_WhenCollectionIsCalculated()
    {
        // Arrange
        var productIds = new List<int> { 1, 2, 3 };
        var orderDate = new DateTime(2024, 7, 7);

        var collectionDate = new CollectionDate { Date = new DateTime(2023, 6, 26) };

        var collectionDateResultDto = new CollectionDateResultDto { Date = new DateTime(2023, 6, 26) };

        var mockCollectionService = new Mock<ICollectionService>();
        mockCollectionService
            .Setup(service => service.CalculateCollectionDateAsync(productIds, orderDate))
            .ReturnsAsync(collectionDate);

        var mockMapper = new Mock<IMapper>();

        var mockLogger = new Mock<ILogger>();

        mockMapper
            .Setup(mapper => mapper.Map<CollectionDateResultDto>(collectionDate))
            .Returns(collectionDateResultDto);

        var controller = new CollectionDateController(mockMapper.Object, mockCollectionService.Object, mockLogger.Object);

        // Act
        var result = await controller.Get(productIds, orderDate);

        // Assert
        var okResult = Assert.IsType<OkObjectResult>(result.Result);
        var returnValue = Assert.IsType<CollectionDateResultDto>(okResult.Value);
        Assert.Equal(collectionDateResultDto.Date, returnValue.Date);
    }

    [Fact]
    public async Task Get_ShouldReturnNotFound_WhenCollectionIsNull()
    {
        // Arrange
        var productIds = new List<int> { 1, 2, 3 };
        var orderDate = new DateTime(2024, 7, 7);

        var mockCollectionService = new Mock<ICollectionService>();
        mockCollectionService
            .Setup(service => service.CalculateCollectionDateAsync(productIds, orderDate))
            .ReturnsAsync((CollectionDate)null);

        var mockMapper = new Mock<IMapper>();

        var mockLogger = new Mock<ILogger>();

        var controller = new CollectionDateController(mockMapper.Object, mockCollectionService.Object, mockLogger.Object);

        // Act
        var result = await controller.Get(productIds, orderDate);

        // Assert
        Assert.IsType<NotFoundResult>(result.Result);
    }

    [Fact]
    public async Task Get_ShouldReturnInternalServerError_WhenExceptionIsThrown()
    {
        // Arrange
        var productIds = new List<int> { 1, 2, 3 };
        var orderDate = new DateTime(2024, 7, 7);

        var mockCollectionService = new Mock<ICollectionService>();
        mockCollectionService
            .Setup(service => service.CalculateCollectionDateAsync(productIds, orderDate))
            .ThrowsAsync(new Exception("Test exception"));

        var mockMapper = new Mock<IMapper>();

        var mockLogger = new Mock<ILogger>();

        var controller = new CollectionDateController(mockMapper.Object, mockCollectionService.Object, mockLogger.Object);

        // Act
        var result = await controller.Get(productIds, orderDate);

        // Assert
        var objectResult = Assert.IsType<ObjectResult>(result.Result);
        Assert.Equal(500, objectResult.StatusCode);
        Assert.Equal("Test exception", objectResult.Value);
        mockLogger.Verify(c => c.Log(It.Is<string>(msg => msg.Contains("Error: "))), Times.Once);
    }
}
