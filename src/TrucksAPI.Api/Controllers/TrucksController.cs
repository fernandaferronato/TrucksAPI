using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using Trucks.Api.Data;
using Trucks.Api.Dtos;
using Trucks.Api.Interfaces;
using Trucks.Api.Models;
using Trucks.Api.Services;

namespace Trucks.Api.Controllers;

[ApiController]
[Route("api/[controller]")]
public class TrucksController : ControllerBase
{
    private readonly ITruckService _service;

    public TrucksController(ITruckService service)
    {
        _service = service;
    }

    // Returns a truck by its ID
    [HttpGet("{id:int}")]
    public async Task<ActionResult<TruckDto>> GetById(int id)
    {
        var truck = await _service.GetByIdAsync(id);
        
        if (truck == null) return NotFound();
        return Ok(ToDto(truck));
    }

    // Returns trucks that match a given chassis ID
    [HttpGet("by-chassis/{chassis}")]
    public async Task<ActionResult<TruckDto>> GetByChassis(string chassis)
    {
        var items = await _service.GetByChassisAsync(chassis);
        
        if (items == null || items.Length == 0) return null;
        var dtos = items.Select(t => ToDto(t));

    return Ok(dtos);
    }

    // Returns all trucks
    [HttpGet]
    public async Task<ActionResult<IEnumerable<TruckDto>>> GetAll()
    {
        var items = await _service.GetAllAsync();
        return Ok(items.Select(ToDto));
    }

    // Creates a new truck
    [HttpPost]
    public async Task<ActionResult<TruckDto>> Create([FromBody] TruckCreateDto dto)
    {
        var entity = new Truck
        {
            Model = dto.Model,
            ManufactureYear = dto.ManufactureYear,
            ChassisId = dto.ChassisId,
            Color = dto.Color,
            Plant = dto.Plant
        };

        try
        {
            var created = await _service.CreateAsync(entity);
            return CreatedAtAction(nameof(GetById), new { id = created.Id }, ToDto(created));
        }
        catch (DbUpdateException)
        {
            //return Conflict(new { message = "ChassisId já cadastrado." });
            return Conflict(new ErrorResponse { Message = "ChassisId já cadastrado." });
        }
    }

    // Updates an existing truck (full update)
    [HttpPut("{id}")]
    public async Task<IActionResult> UpdateTruck(int id, Truck updatedTruck)
    {
        if (id != updatedTruck.Id)
            return BadRequest("ID do caminhão não corresponde ao parâmetro da URL.");

        var existing = await _service.GetByIdAsync(id);
        if (existing == null)
        return NotFound();
        
        existing.Model = updatedTruck.Model;
        existing.ManufactureYear = updatedTruck.ManufactureYear;
        existing.ChassisId = updatedTruck.ChassisId;
        existing.Color = updatedTruck.Color;
        existing.Plant = updatedTruck.Plant;
        
        await _service.UpdateAsync(existing);
        return Ok(ToDto(existing));
    }

    // Deletes a truck by ID
    [HttpDelete("{id:int}")]
    public async Task<IActionResult> Delete(int id)
    {
        var ok = await _service.DeleteAsync(id);
        return ok ? NoContent() : NotFound();
    }

    // Helper method to convert Entity -> DTO
    private static TruckDto ToDto(Truck t) => new(
        t.Id, t.Model, t.ManufactureYear, t.ChassisId, t.Color, t.Plant);
}

public class ErrorResponse
{
    public string Message { get; set; } = string.Empty;
}