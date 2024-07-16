using AutoMapper;
using Microsoft.AspNetCore.Mvc;
using PayPoint.ParcelSystem.Api.Dtos.CollectionsDate;
using PayPoint.ParcelSystem.Domain.Interfaces;
using ILogger = PayPoint.ParcelSystem.Domain.Interfaces.ILogger;

namespace PayPoint.ParcelSystem.Api.Controllers;

[Route("api/[controller]")]
public class CollectionDateController : Controller
{
    private readonly ICollectionService _collectionService;
    private readonly IMapper _mapper;
    private readonly ILogger _logger;

    public CollectionDateController(IMapper mapper, ICollectionService collectionService, ILogger logger)
    {
        _mapper = mapper;
        _collectionService = collectionService;
        _logger = logger;
    }

    [HttpGet]
    [ProducesResponseType<CollectionDateResultDto>(StatusCodes.Status200OK)]
    [ProducesResponseType<CollectionDateResultDto>(StatusCodes.Status404NotFound)]
    [ProducesResponseType<CollectionDateResultDto>(StatusCodes.Status400BadRequest)]
    [ProducesResponseType<CollectionDateResultDto>(StatusCodes.Status500InternalServerError)]
    public async Task<ActionResult<CollectionDateResultDto>> Get(List<int> productIds, DateTime orderDate)
    {
        if (productIds == null || !productIds.Any())
        {
            return BadRequest("ProductIds cannot be null or empty.");
        }

        try
        {
            var collectionDate = await _collectionService.CalculateCollectionDateAsync(productIds, orderDate);

            var collectionDateResultDto = _mapper.Map<CollectionDateResultDto>(collectionDate);

            if (collectionDate == null)
            {
                return NotFound();
            }

            return Ok(collectionDateResultDto);
        }
        catch (Exception ex)
        {
            _logger.Log($"Error: {ex.Message}");
            //  We can include Ilogger here
            return StatusCode(500, ex.Message);
        }
    }

}
