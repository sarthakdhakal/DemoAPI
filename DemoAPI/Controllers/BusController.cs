using System;
using System.Collections;
using System.Threading.Tasks;
using DemoAPI.Models;
using DemoAPI.Services;
using Microsoft.AspNetCore.Mvc;

namespace DemoAPI.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class BusController : Controller
    {
        private readonly IBusServices _busServices;

        public BusController(IBusServices busServices)
        {
            _busServices = busServices;
        }

        [HttpGet]
        public async Task<ActionResult<Car>> GetAll()
        {
            var products = await _busServices.GetBusList();
            Console.WriteLine(products);
            
            return Ok(new { Bus = products.bus, Cars = products.car });
        }

        [HttpGet]
        [Route("{id}")]
        public async Task<ActionResult<Car>> GetById(int id)
        {
            var product = await _busServices.GetBusById(id);
            return Ok(product);
        }
        [HttpPost]
        public async Task<ActionResult> AddBus(Bus entity)
        {
            await _busServices.AddBus(entity);
            return Ok(entity);
        }
        [HttpPut("{id}")]
        public async Task<ActionResult<Bus>> UpdateBus(Bus entity, int id)
        {
            await _busServices.UpdateBus(entity, id);
            return Ok(entity);
        }
        [HttpDelete("{id}")]
        public async Task<ActionResult> Delete(int id)
        {
            await _busServices.RemoveBus(id);
            return Ok();
        }
    }
}