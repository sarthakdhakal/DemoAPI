// using Microsoft.EntityFrameworkCore.Migrations;
//
// #nullable disable
//
// namespace DemoAPI.Migrations
// {
//     public partial class Initial : Migration
//     {
//         protected override void Up(MigrationBuilder migrationBuilder)
//         {
//             migrationBuilder.CreateTable(
//                 name: "Vehicles",
//                 columns: table => new
//                 {
//                     VehicleId = table.Column<int>(type: "int", nullable: false)
//                         .Annotation("SqlServer:Identity", "1, 1"),
//                     VehicleName = table.Column<string>(type: "nvarchar(max)", nullable: false)
//                 },
//                 constraints: table =>
//                 {
//                     table.PrimaryKey("PK_Vehicles", x => x.VehicleId);
//                 });
//
//             migrationBuilder.CreateTable(
//                 name: "Bus",
//                 columns: table => new
//                 {
//                     VehicleId = table.Column<int>(type: "int", nullable: false),
//                     SeatNumber = table.Column<int>(type: "int", nullable: false)
//                 },
//                 constraints: table =>
//                 {
//                     table.PrimaryKey("PK_Bus", x => x.VehicleId);
//                     table.ForeignKey(
//                         name: "FK_Bus_Vehicles_VehicleId",
//                         column: x => x.VehicleId,
//                         principalTable: "Vehicles",
//                         principalColumn: "VehicleId");
//                 });
//
//             migrationBuilder.CreateTable(
//                 name: "Cars",
//                 columns: table => new
//                 {
//                     VehicleId = table.Column<int>(type: "int", nullable: false),
//                     Model = table.Column<string>(type: "nvarchar(max)", nullable: false),
//                     FuelType = table.Column<string>(type: "nvarchar(max)", nullable: false)
//                 },
//                 constraints: table =>
//                 {
//                     table.PrimaryKey("PK_Cars", x => x.VehicleId);
//                     table.ForeignKey(
//                         name: "FK_Cars_Vehicles_VehicleId",
//                         column: x => x.VehicleId,
//                         principalTable: "Vehicles",
//                         principalColumn: "VehicleId");
//                 });
//         }
//
//         protected override void Down(MigrationBuilder migrationBuilder)
//         {
//             migrationBuilder.DropTable(
//                 name: "Bus");
//
//             migrationBuilder.DropTable(
//                 name: "Cars");
//
//             migrationBuilder.DropTable(
//                 name: "Vehicles");
//         }
//     }
// }
