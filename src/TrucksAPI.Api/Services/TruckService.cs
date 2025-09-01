using Microsoft.EntityFrameworkCore;
using Trucks.Api.Data;
using Trucks.Api.Interfaces;
using Trucks.Api.Models;

namespace Trucks.Api.Services;

// Service responsible for managing Truck entities in the database
public class TruckService : ITruckService
{
    private readonly TrucksDbContext _db;

    public TruckService(TrucksDbContext db) => _db = db;

    // Retrieves all trucks
    public async Task<List<Truck>> GetAllAsync() => await _db.Trucks.AsNoTracking().ToListAsync();

    // Retrieves a truck by its ID (returns null if not found)
    public async Task<Truck?> GetByIdAsync(int id) => await _db.Trucks.FindAsync(id);

    // Retrieves trucks by chassis ID (can return multiple trucks)
    public async Task<Truck[]> GetByChassisAsync(string chassis) =>
        await _db.Trucks
                .AsNoTracking()
                .Where(t => t.ChassisId == chassis)
                .ToArrayAsync();

    // Creates a new truck and saves it to the database
    public async Task<Truck> CreateAsync(Truck entity)
    {
        _db.Trucks.Add(entity);
        await _db.SaveChangesAsync();
        return entity;
    }

    public async Task<bool> DeleteAsync(int id)
    {
        var existing = await _db.Trucks.FindAsync(id);
        if (existing is null) return false;
        _db.Trucks.Remove(existing);
        await _db.SaveChangesAsync();
        return true;
    }

    public async Task<Truck?> UpdateAsync(Truck entity)
    {
        var existing = await _db.Trucks.FindAsync(entity.Id);
        if (existing == null) return null;
        
        _db.Entry(existing).CurrentValues.SetValues(entity);
        await _db.SaveChangesAsync();
        return existing;
    }

    // public async Task<Truck?> UpdatePartialAsync(int id, Action<Truck> apply)
    // {
    //     var entity = await _db.Trucks.FindAsync(id);
    //     if (entity is null) return null;
    //     apply(entity);
    //     await _db.SaveChangesAsync();
    //     return entity;
    // }
}