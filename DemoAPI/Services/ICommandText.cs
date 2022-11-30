using System;

namespace DemoAPI.Services
{
    public interface ICommandText
    {
        string GetCars { get; }
        string GetCarById { get; }
        string AddCar { get; }
        string UpdateCar { get; }
        string RemoveCar { get; }
        string GetBuses { get; }
        string GetBusById { get; }
        string AddBus { get; }
        string AddVehicle { get; }
        string UpdateBus { get; }
        string RemoveBus { get; }
        string UpdateVehicle { get; }
        string RemoveVehicles { get; }
        string LastAddedVehicle { get; }
        string GetUsers { get; }
        
        string RegisterUser { get; }
    }


}