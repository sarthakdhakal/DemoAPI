using System;
using System.Collections.Generic;
using System.Data;
using System.Threading.Tasks;
using Dapper;
using DemoAPI.Models;
using BC = BCrypt.Net.BCrypt;
using Microsoft.Extensions.Configuration;

namespace DemoAPI.Services
{
    public class UserServices : BaseRepository, IUserServices
    {
        private readonly ICommandText _commandText;
        public UserServices(IConfiguration configuration, ICommandText commandText) : base(configuration)
        {
            _commandText = commandText;
        }


        public Task AddUser(DataTable dataTable)
        {
            return WithConnection(async conn =>
            {
                using (var tran = conn.BeginTransaction())
                {

                    try
                    {
                      


                            

                            await conn.ExecuteAsync(_commandText.RegisterUser, new { DataTable = dataTable.AsTableValuedParameter("DataTable") }, transaction: tran
                            );

                        

                        tran.Commit();
                    }
                    catch (Exception e)
                    {
                        tran.Rollback();
                    }
                }
            });
        }

        public Task<IEnumerable<User>> GetUserList()
        {
            return WithConnection(async conn =>
            {
                var query = await conn.QueryAsync<User>(_commandText.GetUsers);
                return query;
            });

        }
    }
}