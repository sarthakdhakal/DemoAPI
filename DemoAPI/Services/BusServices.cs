using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Dapper;
using DemoAPI.Models;
using Microsoft.Extensions.Configuration;

namespace DemoAPI.Services
{
    public class BusServices : BaseRepository, IBusServices
    {
        private readonly ICommandText _commandText;

        public BusServices(IConfiguration configuration, ICommandText commandText) : base(configuration)
        {
            _commandText = commandText;
        }


        public async Task<(IEnumerable<Bus> bus, IEnumerable<Car> car)> GetBusList()
        {
            return await WithConnection(async conn =>
            {
                
                var query = await conn.QueryMultipleAsync(_commandText.GetBuses);
                var bus = query.Read<Bus>();
                var car = query.Read<Car>();
                return (bus, car);
               
            });
        }

        public async ValueTask<Bus> GetBusById(int id)
        {
            return await WithConnection(async conn =>
            {
               
                    var query = await conn.QuerySingleOrDefaultAsync<Bus>(_commandText.GetBusById, new {VehicleId = id});
                   
                    return query;
                
            });
        }

        public async Task AddBus(Bus entity)
        {
            Vehicle vehicle = new Vehicle();
            await WithConnection(async conn =>
            {
                using (var tran = conn.BeginTransaction())
                {
                 
                        await conn.ExecuteAsync(_commandText.AddVehicle,
                            new {VehicleName = entity.VehicleName}, transaction: tran);
                        vehicle = await conn.QueryFirstAsync<Vehicle>(_commandText.LastAddedVehicle, transaction: tran);
                        int id = vehicle.VehicleId;
                        await conn.ExecuteAsync(_commandText.AddBus,
                            new {VehicleId = id, SeatNumber = entity.SeatNumber}, transaction: tran);
                        tran.Commit();
                    }
                
                
            });
        }

        public async Task UpdateBus(Bus entity, int id)
        {
            await WithConnection(async conn =>
            {
                using (var tran = conn.BeginTransaction())
                {
                    try
                    {
                        await conn.ExecuteAsync(_commandText.UpdateVehicle,
                            new
                            {
                                VehicleName = entity.VehicleName,
                                VehicleId = id
                            }, transaction: tran);
                        await conn.ExecuteAsync(_commandText.UpdateBus,
                            new {SeatNumber = entity.SeatNumber, VehicleId = id}, transaction: tran);
                    }
                    catch (Exception e)
                    {
                        tran.Rollback();
                    }
                }
            });
        }

        public async Task RemoveBus(int id)
        {
            await WithConnection(async conn =>
            {
                using (var tran = conn.BeginTransaction())
                {
                    try
                    {
                        await conn.ExecuteAsync(_commandText.RemoveBus, new {VehicleId = id}, transaction: tran);
                        await conn.ExecuteAsync(_commandText.RemoveCar, new {VehicleId = id}, transaction: tran);
                        tran.Commit();
                    }
                    catch (Exception e)
                    {
                        tran.Rollback();
                    }
                }
            });
        }
    }
}