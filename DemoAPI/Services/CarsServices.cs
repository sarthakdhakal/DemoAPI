using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Dapper;
using DemoAPI.Models;
using Microsoft.Extensions.Configuration;

namespace DemoAPI.Services
{
    public class CarsServices : BaseRepository, ICarsServices
    {
        private readonly ICommandText _commandText;

        public CarsServices(IConfiguration configuration, ICommandText commandText) : base(configuration)
        {
            _commandText = commandText;
        }

        public async Task<IEnumerable<Car>> GetCarsList()
        {
            return await WithConnection(async conn =>
            {
               
                    var query = await conn.QueryAsync<Car>(_commandText.GetCars);
                    
                    return query;
                
            });
        }

        public async ValueTask<Car> GetCarById(int id)
        {
            return await WithConnection(async conn =>
            {
                
                    var query = await conn.QueryFirstOrDefaultAsync<Car>(_commandText.GetCarById, new {VehicleId = id});
                 
                    return query;
               
            });
        }

        public async Task AddCar(Car entity)
        {
            Vehicle vehicle = new Vehicle();
            await WithConnection(async conn =>
            {
                using (var tran = conn.BeginTransaction())
                {
                    try
                    {
                        await conn.ExecuteAsync(_commandText.AddVehicle,
                            new {VehicleName = entity.VehicleName}, transaction: tran);
                        vehicle = await conn.QueryFirstAsync<Vehicle>(_commandText.LastAddedVehicle, transaction: tran);
                        int id = vehicle.VehicleId;
                        await conn.ExecuteAsync(_commandText.AddCar,
                            new {VehicleId = id, Model = entity.Model, FuelType = entity.FuelType}, transaction: tran);
                        tran.Commit();
                    }
                    catch (Exception e)
                    {
                        tran.Rollback();
                    }
                }
            });
        }

        public async Task UpdateCar(Car entity, int id)
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
                        await conn.ExecuteAsync(_commandText.UpdateCar,
                            new
                            {
                                Model = entity.Model, FuelType = entity.FuelType,
                                VehicleId = id
                            }, transaction: tran);
                        tran.Commit();
                    }
                    catch (Exception e)
                    {
                        tran.Rollback();
                    }
                }
            });
        }

        public async Task RemoveCar(int id)
        {
            await WithConnection(async conn =>
            {
                using (var tran = conn.BeginTransaction())
                {
                    try
                    {
                        await conn.ExecuteAsync(_commandText.RemoveCar, new {VehicleId = id}, transaction: tran);
                        await conn.ExecuteAsync(_commandText.RemoveVehicles, new {VehicleId = id}, transaction: tran);
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