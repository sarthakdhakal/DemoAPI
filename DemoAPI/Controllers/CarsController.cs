using System.Threading.Tasks;
using DemoAPI.Models;
using DemoAPI.Services;
using Microsoft.AspNetCore.Mvc;

namespace DemoAPI.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class CarsController : ControllerBase
    {
        private readonly ICarsServices _carsServices;

        public CarsController(ICarsServices carsServices)
        {
            _carsServices = carsServices;
        }

   
        [HttpGet]
        public async Task<ActionResult<Car>> GetAll()
        {
            var products = await _carsServices.GetCarsList();
            return Ok(products);
        }

        [HttpGet]
        [Route("{id}")]
        public async Task<ActionResult<Car>> GetById(int id)
        {
            var product = await _carsServices.GetCarById(id);
            return Ok(product);
        }
        [HttpPost]
        public async Task<ActionResult> AddCar(Car entity)
        {
            await _carsServices.AddCar(entity);
            return Ok(entity);
        }
        [HttpPut("{id}")]
        public async Task<ActionResult<Car>> Update(Car entity, int id)
        {
            await _carsServices.UpdateCar(entity, id);
            return Ok(entity);
        }
        [HttpDelete("{id}")]
        public async Task<ActionResult> Delete(int id)
        {
            await _carsServices.RemoveCar(id);
            return Ok();
        }
    }
}