using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace DemoAPI.Models
{

    public class Vehicle
    {
        [Key]
        public int VehicleId { get; set; }

        public string VehicleName { get; set; }
    }
}