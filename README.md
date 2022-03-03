Installation and execution

Open Project and Check SqlServer ConnectionString

if necessary change in the file: Elevator.Management.Api\appsettings.json

INITIALIZE DATABASE

1. Set as startup project Elevator.Management.Api

(IMPORTANT) In Package Manager Console:

2. Select as default proyect Elevator.Management.Persistence
3. `update-database -Context ElevatorDbContext`
4. Select as default proyect Elevator.Management.Identity
5. `update-database -Context ElevatorIdentityDbContext`

Run Unit and Integration Tests

1. `Ctrl + R + T`

Build and Run Project

1. Click on Run with IIS Express
2. Open your browser on https://localhost:44330/swagger
3. Test the api with swagger or postman

In Terminal Path: Elevator.Management.Api

Second option to run

1. `dotnet build`
2. `dotnet run`
3. Open your browser on https://localhost:5001/swagger/index.html
4. Test the api with swagger or postman
