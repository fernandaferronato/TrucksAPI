using Trucks.Api.Models;

namespace Trucks.Api.Dtos;

public record TruckDto(
    int Id,
    TruckModel Model,
    int ManufactureYear,
    string ChassisId,
    string Color,
    Plant Plant
);