using System.Collections.Generic;
using System.Data;
using System.Dynamic;
using System.IO;
using System.Linq;
using System.Text.Json;
using System.Threading.Tasks;
using ClosedXML.Excel;
using DemoAPI.Models;
using DemoAPI.Services;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;


namespace DemoAPI.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class UserController : ControllerBase
    {
        private readonly IUserServices _userServices;
        private readonly ICSVService _csvService;
        public UserController(IUserServices userServices,ICSVService csvService)
        {
            _userServices = userServices;
            _csvService = csvService;
        }
        // GET: api/<UserController>

        [HttpGet]
        public async Task<FileContentResult> GetAll()
        {
            var products = await _userServices.GetUserList();
            var data= await _csvService.ReturnFile(products);
           
                    return File(
                        data,
                        "application/xlsx",
                        "users.xlsx");
                

            }
        

        // POST api/<UserController>
        [HttpPost]
        public async Task<ActionResult> Post([FromForm] IFormFileCollection file)

        {
            var users =  _csvService.ReadXLSX(file);
            
            await _userServices.AddUser(users);
            return Ok(users);
            
        }
      
    }
}
