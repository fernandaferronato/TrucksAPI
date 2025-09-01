using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace Trucks.Api.Models;

public class Truck
{
    public int Id { get; set; }

    [Required]
    [RegularExpression("^(FH|FM|VM)$", ErrorMessage = "Modelo deve ser FH, FM ou VM.")]
    public TruckModel Model { get; set; }
    
    [Required]
    public int ManufactureYear { get; set; }
    
    [Required]
    [StringLength(30)]
    public string ChassisId { get; set; } = default!;

    [Required]
    [StringLength(40)]
    public string Color { get; set; } = default!;

    [Required]
    public Plant Plant { get; set; }
}