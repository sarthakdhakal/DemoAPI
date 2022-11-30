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


        public Task AddUser(IList<User> entity)
        {
            return WithConnection(async conn =>
            {
                using (var tran = conn.BeginTransaction())
                {

                    try
                    {

                        foreach (var user in entity)
                        {

                            var hashPassword = BC.HashPassword(user.Password);


                            await conn.ExecuteAsync(_commandText.RegisterUser, new { UserName =user.UserName, Name= user.Name, Email= user.Email, Password= hashPassword, Role = user.Role }, transaction: tran
                            );

                        } 

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