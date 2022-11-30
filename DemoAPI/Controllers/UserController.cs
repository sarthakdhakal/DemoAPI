using System.Collections.Generic;
using System.Data;
using System.Dynamic;
using System.Linq;
using System.Text.Json;
using System.Threading.Tasks;
using ClosedXML.Excel;
using DemoAPI.Models;
using DemoAPI.Services;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;

// For more information on enabling Web API for empty projects, visit https://go.microsoft.com/fwlink/?LinkID=397860

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
        public async Task<ActionResult<User>> GetAll()
        {
            var products = await _userServices.GetUserList();
            return Ok(products);
        }
        // POST api/<UserController>
        [HttpPost]
        public async Task<ActionResult> Post([FromForm] IFormFileCollection file)

        {
            using var workbook = new XLWorkbook(file[0].OpenReadStream());
            var ws = workbook.Worksheet(1);

            var firstRowUsed = ws.FirstRowUsed();
            var firstPossibleAddress = ws.Row(firstRowUsed.RowNumber()).FirstCell().Address;
            var lastPossibleAddress = ws.LastCellUsed().Address;

            // Get a range with the remainder of the worksheet data (the range used)
            var range = ws.Range(firstPossibleAddress, lastPossibleAddress).AsRange(); //.RangeUsed();
                                                                                              // Treat the range as a table (to be able to use the column names)
            var table = range.AsTable();

            //Specify what are all the Columns you need to get from Excel
            var dataList = new List<string[]>
                {
                    table.DataRange.Rows()
                        .Select(tableRow =>
                            tableRow.Field("UserName")
                                .GetString())
                        .ToArray(),
                    table.DataRange.Rows()
                        .Select(tableRow => tableRow.Field("Name").GetString())
                        .ToArray(),
                    table.DataRange.Rows()
                    .Select(tableRow => tableRow.Field("Email").GetString())
                    .ToArray(),

                    table.DataRange.Rows()
                    .Select(tableRow => tableRow.Field("Password").GetString())
                    .ToArray()
                    ,
                    table.DataRange.Rows()
                    .Select(tableRow => tableRow.Field("Role").GetString())
                    .ToArray()
                };
            ////Convert List to DataTable
            //var dataTable = ConvertListToDataTable(dataList);
            ////To get unique column values, to avoid duplication
            //var uniqueCols = dataTable.DefaultView.ToTable(true, "Solution Number");
            //List<User> users = new List<User>();
            //for (int i = 0; i < dataList.Count; i++)
            //{

            //    users.Add(new Models.User
            //    {
            //        UserName = dataList[0].ToString(),
            //        Name = dataList[1].ToString(),
            //        Email = dataList[2].ToString(),
            //        Password = dataList[3].ToString(),
            //        Role = dataList[4].ToString()
            //    }) ;  


            //}

            //Get the first sheet of workbook
            // var worksheet = workbook.Worksheet(1);
            //
            // var firstRowUsed = worksheet.FirstRowUsed();
            // var categoryRow = firstRowUsed.RowUsed();
            //
            // int coCategoryId = 1;
            //
            // //Get the column names from first row of excel
            // Dictionary<int, string> keyValues = new Dictionary<int, string>();
            // for (int cell = 1; cell <= categoryRow.CellCount(); cell++)
            // {
            //     keyValues.Add(cell, categoryRow.Cell(cell).GetString());
            // }
            //
            // //Get the next row
            // categoryRow = categoryRow.RowBelow();
            // while (!categoryRow.Cell(coCategoryId).IsEmpty())
            // {
            //     int count = 1;
            //     var pc = new ExpandoObject();
            //     User user = new User();
            //     while (count <= categoryRow.CellCount())
            //     {
            //         // let this go through-if the data is bad, it will be rejected by SQL
            //         var data = categoryRow.Cell(count).Value;
            //
            //         ((IDictionary<string, object>)pc).Add(keyValues[count], data);
            //         var data= pc.Where(pair => pair.Key == keyValues[count]).Select(pair => pair.Value).ToString;
            //         count++;
            //         switch (count)
            //         {
            //             case 1:
            //                 user.UserName = (string)data;
            //                 break;
            //             case 2:
            //                 user.Name = (string)data;
            //                 break;
            //             case 3:
            //                 user.Email = (string)data;
            //                 break;
            //             case 4:
            //                 user.Password = (string)data;
            //                 break;
            //             case 5:
            //                 user.Role = (string)data;
            //                 break;
            //             
            //         }
            //     }
            //     categoryRow = categoryRow.RowBelow();
            // }
            DataTable dataTable = ConvertListToDataTable(dataList);
            //IList<User> users = JsonSerializer.Deserialize<User>(dataList);

            await _userServices.AddUser(dataTable);
            return Ok();
        }
        private static DataTable ConvertListToDataTable(IReadOnlyList<string[]> list)
        {
            var table = new DataTable("CustomTable");
            var rows = list.Select(array => array.Length).Concat(new[] { 0 }).Max();
            
            table.Columns.Add("UserName");
            table.Columns.Add("Name");
            table.Columns.Add("Email");
            table.Columns.Add("Password");
            table.Columns.Add("Role");

            
            for (var j = 0; j < rows; j++)
            {
                var row = table.NewRow();
                row["UserName"] = list[0][j];
                row["Name"] = list[1][j];
                row["Email"] = list[2][j];
                row["Password"] = list[3][j];
                row["Role"] = list[4][j];

                
                table.Rows.Add(row);
            }
            return table;
        }

    }
}
