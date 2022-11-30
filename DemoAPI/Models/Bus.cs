using System.ComponentModel.DataAnnotations.Schema;

namespace DemoAPI.Models
{
    [Table("Bus")]
    public class Bus:Vehicle
    {
        public int SeatNumber { get; set; }
    }
}