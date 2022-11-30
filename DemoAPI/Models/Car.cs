using System;
using System.ComponentModel.DataAnnotations.Schema;
using Microsoft.AspNetCore.Mvc;

namespace DemoAPI.Models
{
    [Table("Cars")]
    public class Car:Vehicle
    {
        public string Model { get; set; }
        public string FuelType { get; set; }
    }
}