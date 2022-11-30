using System.Collections;
using System.Collections.Generic;
using System.Data;
using System.Threading.Tasks;
using DemoAPI.Models;

namespace DemoAPI.Services
{
    public interface IUserServices
    {
        
        Task<IEnumerable<User>> GetUserList();
        Task AddUser(DataTable dataTable);
    }
}