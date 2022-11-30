using System.Collections.Generic;
using System.Threading.Tasks;
using Dapper;
using DemoAPI.Models;

namespace DemoAPI.Services
{
    public interface IBusServices
    {

        ValueTask<Bus> GetBusById(int id);
        Task AddBus(Bus entity);
        Task UpdateBus(Bus entity, int id);
        Task RemoveBus(int id);
        Task<(IEnumerable<Bus> bus, IEnumerable<Car> car)> GetBusList();
    }
}