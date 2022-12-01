using System.Collections;
using System.Collections.Generic;
using System.IO;
using System.Threading.Tasks;
using DemoAPI.Models;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;

namespace DemoAPI.Services
{
    public interface ICSVService
    {
        public IEnumerable<T> ReadCSV<T>(Stream file);
        public IList<User> ReadXLSX(IFormFileCollection file);

        public Task<byte[]> ReturnFile(IEnumerable<User> users);
    }
}
