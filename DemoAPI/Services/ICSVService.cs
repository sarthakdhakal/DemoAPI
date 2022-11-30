using System.Collections.Generic;
using System.IO;

namespace DemoAPI.Services
{
    public interface ICSVService
    {
        public IEnumerable<T> ReadCSV<T>(Stream file);
    }
}
