using System.Collections;
using System.Collections.Generic;
using System.Data;
using CsvHelper;
using System.Globalization;
using System.IO;
using System.Linq;
using System.Threading.Tasks;
using ClosedXML.Excel;
using DemoAPI.Models;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;

namespace DemoAPI.Services
{
    public class CSVService : ICSVService
    {
        public IEnumerable<T> ReadCSV<T>(Stream file)
        {
            var reader = new StreamReader(file);
            var csv = new CsvReader(reader, CultureInfo.InvariantCulture);

            var records = csv.GetRecords<T>();
            return records;
        }

        public  IList<User> ReadXLSX(IFormFileCollection file)
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
                    .ToArray(),
                table.DataRange.Rows()
                    .Select(tableRow => tableRow.Field("Role").GetString())
                    .ToArray()
            };
            DataTable dataTable = ConvertListToDataTable(dataList);
            IList<User> users = new List<User>();
            for (int i = 0; i < dataTable.Rows.Count; i++)
            {

                users.Add(new User
                {
                    UserName = dataTable.Rows[i].Field<string>("UserName"),
                    Name = dataTable.Rows[i].Field<string>("Name"),
                    Email = dataTable.Rows[i].Field<string>("Email"),
                    Password = dataTable.Rows[i].Field<string>("Password"),
                    Role = dataTable.Rows[i].Field<string>("Role"),

                });
            }

            return users;
        }

        public async Task<byte[]> ReturnFile(IEnumerable<User> users)
        {
            using (var workbook = new XLWorkbook())
            {
                var worksheet = workbook.Worksheets.Add("Users");
                var currentRow = 1;
                worksheet.Cell(currentRow, 1).Value = "Id";
                worksheet.Cell(currentRow, 2).Value = "Username";
                worksheet.Cell(currentRow, 3).Value = "Name";
                worksheet.Cell(currentRow, 4).Value = "Email";
                worksheet.Cell(currentRow, 5).Value = "Password";
                worksheet.Cell(currentRow, 6).Value = "Role";

                foreach (var user in users)
                {
                    currentRow++;
                    worksheet.Cell(currentRow, 1).Value = user.UserId;
                    worksheet.Cell(currentRow, 2).Value = user.UserName;

                    worksheet.Cell(currentRow, 3).Value = user.Name;
                    worksheet.Cell(currentRow, 4).Value = user.Email;
                    worksheet.Cell(currentRow, 5).Value = user.Password;
                    worksheet.Cell(currentRow, 6).Value = user.Role;
                }

                await using (var stream = new MemoryStream())
                {
                    workbook.SaveAs(stream);
                    var content = stream.ToArray();

                    return content;
                }
            }
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

