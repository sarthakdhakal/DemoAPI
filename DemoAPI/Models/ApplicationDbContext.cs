using Microsoft.EntityFrameworkCore;

namespace DemoAPI.Models
{
    public class ApplicationDbContext:DbContext
    { 
        public ApplicationDbContext(DbContextOptions<ApplicationDbContext> options)
            : base(options)
        {
        }

        public virtual DbSet<Vehicle> Vehicles { get; set; }
        public virtual DbSet<Car> Cars { get; set; }
        public virtual DbSet<Bus> Buses { get; set; }
        public virtual DbSet<User> Users { get; set; }


    }
}