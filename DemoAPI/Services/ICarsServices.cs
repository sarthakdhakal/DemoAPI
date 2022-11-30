using System.Collections.Generic;
using System.Threading.Tasks;
using DemoAPI.Models;
using Microsoft.AspNetCore.Mvc;

namespace DemoAPI.Services
{
    public interface ICarsServices
    {

     
        ValueTask<Car> GetCarById(int id);
        Task AddCar(Car entity);
        Task UpdateCar(Car entity, int id);
        Task RemoveCar(int id);
        Task<IEnumerable<Car>> GetCarsList();
    }
}