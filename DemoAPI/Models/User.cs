using System.ComponentModel;
using System.ComponentModel.DataAnnotations;

namespace DemoAPI.Models
{
    public class User
    {
        [Key] 
        public int UserId { get; set; }
        public string Name { get; set; }
        public string Email { get; set; }
        public string Role { get; set; }
        public string Password { get; set; }
        public string UserName { get; set; }
    }
}