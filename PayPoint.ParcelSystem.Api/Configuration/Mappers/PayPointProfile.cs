using AutoMapper;
using PayPoint.ParcelSystem.Api.Dtos.CollectionsDate;
using PayPoint.ParcelSystem.Domain.Models;

namespace PayPoint.ParcelSystem.Api.Configuration.Mappers;

public class PayPointProfile : Profile
{
    public PayPointProfile()
    {
        CreateMap<CollectionDate, CollectionDateResultDto>().ReverseMap();
    }
}
