using System;
using Microsoft.EntityFrameworkCore.Migrations;

namespace Elevator.Management.Persistence.Migrations
{
    public partial class InitialMigration : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.CreateTable(
                name: "Buildings",
                columns: table => new
                {
                    BuildingId = table.Column<Guid>(nullable: false),
                    CreatedBy = table.Column<string>(nullable: true),
                    CreatedDate = table.Column<DateTime>(nullable: false),
                    LastModifiedBy = table.Column<string>(nullable: true),
                    LastModifiedDate = table.Column<DateTime>(nullable: true),
                    Name = table.Column<string>(nullable: true),
                    FirstFloor = table.Column<int>(nullable: false),
                    LastFloor = table.Column<int>(nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Buildings", x => x.BuildingId);
                });

            migrationBuilder.CreateTable(
                name: "Elevators",
                columns: table => new
                {
                    ElevatorId = table.Column<Guid>(nullable: false),
                    CreatedBy = table.Column<string>(nullable: true),
                    CreatedDate = table.Column<DateTime>(nullable: false),
                    LastModifiedBy = table.Column<string>(nullable: true),
                    LastModifiedDate = table.Column<DateTime>(nullable: true),
                    BuildingId = table.Column<Guid>(nullable: false),
                    Name = table.Column<string>(nullable: true),
                    State = table.Column<int>(nullable: false),
                    CurrentFloor = table.Column<int>(nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Elevators", x => x.ElevatorId);
                });

            migrationBuilder.CreateTable(
                name: "Movements",
                columns: table => new
                {
                    MovementId = table.Column<Guid>(nullable: false),
                    CreatedBy = table.Column<string>(nullable: true),
                    CreatedDate = table.Column<DateTime>(nullable: false),
                    LastModifiedBy = table.Column<string>(nullable: true),
                    LastModifiedDate = table.Column<DateTime>(nullable: true),
                    ElevatorId = table.Column<Guid>(nullable: false),
                    DestinationFloor = table.Column<int>(nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Movements", x => x.MovementId);
                    table.ForeignKey(
                        name: "FK_Movements_Elevators_ElevatorId",
                        column: x => x.ElevatorId,
                        principalTable: "Elevators",
                        principalColumn: "ElevatorId",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.InsertData(
                table: "Buildings",
                columns: new[] { "BuildingId", "CreatedBy", "CreatedDate", "FirstFloor", "LastFloor", "LastModifiedBy", "LastModifiedDate", "Name" },
                values: new object[] { new Guid("b0788d2f-8003-43c1-92a4-edc76a7c5dde"), null, new DateTime(1, 1, 1, 0, 0, 0, 0, DateTimeKind.Unspecified), -2, 14, null, null, "Masiv" });

            migrationBuilder.InsertData(
                table: "Elevators",
                columns: new[] { "ElevatorId", "BuildingId", "CreatedBy", "CreatedDate", "CurrentFloor", "LastModifiedBy", "LastModifiedDate", "Name", "State" },
                values: new object[,]
                {
                    { new Guid("6313179f-7837-473a-a4d5-a5571b43e6a6"), new Guid("b0788d2f-8003-43c1-92a4-edc76a7c5dde"), null, new DateTime(1, 1, 1, 0, 0, 0, 0, DateTimeKind.Unspecified), -1, null, null, "A", 1 },
                    { new Guid("bf3f3002-7e53-441e-8b76-f6280be284aa"), new Guid("b0788d2f-8003-43c1-92a4-edc76a7c5dde"), null, new DateTime(1, 1, 1, 0, 0, 0, 0, DateTimeKind.Unspecified), 11, null, null, "B", 2 },
                    { new Guid("fe98f549-e790-4e9f-aa16-18c2292a2ee9"), new Guid("b0788d2f-8003-43c1-92a4-edc76a7c5dde"), null, new DateTime(1, 1, 1, 0, 0, 0, 0, DateTimeKind.Unspecified), 4, null, null, "C", 0 }
                });

            migrationBuilder.InsertData(
                table: "Movements",
                columns: new[] { "MovementId", "CreatedBy", "CreatedDate", "DestinationFloor", "ElevatorId", "LastModifiedBy", "LastModifiedDate" },
                values: new object[,]
                {
                    { new Guid("830be110-0361-4cf9-8004-2a1693aad8f7"), null, new DateTime(1, 1, 1, 0, 0, 0, 0, DateTimeKind.Unspecified), 3, new Guid("6313179f-7837-473a-a4d5-a5571b43e6a6"), null, null },
                    { new Guid("2d7f6e52-007e-4c23-a863-85e16d829d1a"), null, new DateTime(1, 1, 1, 0, 0, 0, 0, DateTimeKind.Unspecified), 2, new Guid("6313179f-7837-473a-a4d5-a5571b43e6a6"), null, null },
                    { new Guid("64c54d08-1349-4430-b39c-9537a044dbf5"), null, new DateTime(1, 1, 1, 0, 0, 0, 0, DateTimeKind.Unspecified), 4, new Guid("6313179f-7837-473a-a4d5-a5571b43e6a6"), null, null },
                    { new Guid("16a80793-4576-4534-be86-fb6fafa9f0fe"), null, new DateTime(1, 1, 1, 0, 0, 0, 0, DateTimeKind.Unspecified), 5, new Guid("bf3f3002-7e53-441e-8b76-f6280be284aa"), null, null },
                    { new Guid("23ec86e9-510d-4e76-8c5d-b769c36ed01f"), null, new DateTime(1, 1, 1, 0, 0, 0, 0, DateTimeKind.Unspecified), 8, new Guid("bf3f3002-7e53-441e-8b76-f6280be284aa"), null, null }
                });

            migrationBuilder.CreateIndex(
                name: "IX_Movements_ElevatorId",
                table: "Movements",
                column: "ElevatorId");
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropTable(
                name: "Buildings");

            migrationBuilder.DropTable(
                name: "Movements");

            migrationBuilder.DropTable(
                name: "Elevators");
        }
    }
}
