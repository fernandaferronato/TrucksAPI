using System.ComponentModel.DataAnnotations;
using Trucks.Api.Models;

namespace Trucks.Api.Dtos;

public class TruckUpdateDto
{
    [Required]
    [RegularExpression("^(FH|FM|VM)$")]
    public TruckModel? Model { get; set; }

    [Required]
    public int? ManufactureYear { get; set; }

    [Required]
    [StringLength(30)]
    public string ChassisId { get; set; } = default!;

    [StringLength(40)]
    public string? Color { get; set; }

    public Plant? Plant { get; set; }
}