namespace DemoAPI.Services
{
    public class CommandText:ICommandText
    {
        public string GetCars => "Select * From Cars c join Vehicles p on c.VehicleId=p.VehicleId";
        public string GetCarById => "Select * From Cars c join Vehicles p on c.VehicleId=p.VehicleId And c.VehicleId = @VehicleId";
        public string AddCar => "Insert Into Cars (VehicleId,Model, FuelType) Values (@VehicleId, @Model,@FuelType)";
        public string AddVehicle => "Insert Into Vehicles (VehicleName) Values (@VehicleName)";
        public string UpdateCar => "Update Cars set Model = @Model, FuelType = @FuelType Where VehicleId =@VehicleId";
        public string UpdateVehicle => "Update Vehicles set VehicleName = @VehicleName Where VehicleId =@VehicleId";
        public string RemoveCar => "Delete From Cars Where VehicleId= @VehicleId";
        public string RemoveVehicles => "Delete From Vehicles Where VehicleId= @VehicleId";      
        public string GetBuses => "Select * From Bus  c join Vehicles p on c.VehicleId=p.VehicleId; Select * From Cars  c join Vehicles p on c.VehicleId=p.VehicleId";       
        public string GetBusById => "Select * From Bus c join Vehicles p on c.VehicleId=p.VehicleId AND c.VehicleId = @VehicleId";
        public string AddBus => "Insert Into Bus (VehicleId, SeatNumber) Values (@VehicleId, @SeatNumber)";
        public string UpdateBus => "Update Bus set  SeatNumber = @SeatNumber Where VehicleId =@VehicleId";
        public string RemoveBus => "Delete From Bus Where VehicleId= @VehicleId";
        public string LastAddedVehicle => "Select top 1* From Vehicles order by VehicleId desc";
        
        public string RegisterUser => "Insert Into Users (Username,Name, Email,Role,Password) SELECT Username, Name, Email,Role,Password FROM @DataTable";

        public string GetUsers => "Select * from Users";
    }
}   
