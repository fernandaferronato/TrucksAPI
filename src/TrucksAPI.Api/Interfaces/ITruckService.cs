using Trucks.Api.Models;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace Trucks.Api.Interfaces
{
    public interface ITruckService
    {
        Task<Truck?> GetByIdAsync(int id);
        Task<Truck[]> GetByChassisAsync(string chassisId);
        Task<List<Truck>> GetAllAsync();
        Task<Truck> CreateAsync(Truck truck);
        Task<Truck?> UpdateAsync(Truck truck);
        Task<bool> DeleteAsync(int id);
    }
}