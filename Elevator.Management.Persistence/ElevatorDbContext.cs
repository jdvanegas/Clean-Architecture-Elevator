using Elevator.Management.Application.Contracts;
using Elevator.Management.Domain.Common;
using Elevator.Management.Domain.Entities;
using Microsoft.EntityFrameworkCore;
using System;
using System.Threading;
using System.Threading.Tasks;

namespace Elevator.Management.Persistence
{
    public class ElevatorDbContext: DbContext
    {
        private readonly ILoggedInUserService _loggedInUserService;

        public ElevatorDbContext(DbContextOptions<ElevatorDbContext> options)
           : base(options)
        {
        }

        public ElevatorDbContext(DbContextOptions<ElevatorDbContext> options, ILoggedInUserService loggedInUserService)
            : base(options)
        {
            _loggedInUserService = loggedInUserService;
        }

        public DbSet<Domain.Entities.Elevator> Elevators { get; set; }
        public DbSet<Movement> Movements { get; set; }
        public DbSet<Building> Buildings { get; set; }

        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            modelBuilder.ApplyConfigurationsFromAssembly(typeof(ElevatorDbContext).Assembly);

            //seed data, added through migrations
            var buildingId = Guid.Parse("{B0788D2F-8003-43C1-92A4-EDC76A7C5DDE}");
            var elevatorAId = Guid.Parse("{6313179F-7837-473A-A4D5-A5571B43E6A6}");
            var elevatorBId = Guid.Parse("{BF3F3002-7E53-441E-8B76-F6280BE284AA}");
            var elevatorCId = Guid.Parse("{FE98F549-E790-4E9F-AA16-18C2292A2EE9}");

            modelBuilder.Entity<Building>().HasKey(e => e.BuildingId);
            modelBuilder.Entity<Domain.Entities.Elevator>().HasKey(e => e.ElevatorId);
            modelBuilder.Entity<Movement>().HasKey(e => e.MovementId);          

            modelBuilder.Entity<Building>().HasData(new Building
            {
                BuildingId = buildingId,
                Name = "Masiv",
                FirstFloor = -2,
                LastFloor = 14,
            });

            modelBuilder.Entity<Domain.Entities.Elevator>().HasData(new Domain.Entities.Elevator
            {
                ElevatorId = elevatorAId,
                BuildingId = buildingId,
                Name = "A",
                CurrentFloor = -1,
                State = Domain.Enums.ElevatorState.GoingUp
            });

            modelBuilder.Entity<Domain.Entities.Elevator>().HasData(new Domain.Entities.Elevator
            {
                ElevatorId = elevatorBId,
                BuildingId = buildingId,
                Name = "B",
                CurrentFloor = 11,
                State = Domain.Enums.ElevatorState.GoingDown
            });
            modelBuilder.Entity<Domain.Entities.Elevator>().HasData(new Domain.Entities.Elevator
            {
                ElevatorId = elevatorCId,
                BuildingId = buildingId,
                Name = "C",
                CurrentFloor = 4,
                State = Domain.Enums.ElevatorState.Still
            });


            modelBuilder.Entity<Movement>().HasData(new Movement
            {
                ElevatorId = elevatorAId,
                DestinationFloor = 3
            });
            modelBuilder.Entity<Movement>().HasData(new Movement
            {
                ElevatorId = elevatorAId,
                DestinationFloor = 2
            });
            modelBuilder.Entity<Movement>().HasData(new Movement
            {
                ElevatorId = elevatorAId,
                DestinationFloor = 4
            });
            modelBuilder.Entity<Movement>().HasData(new Movement
            {
                ElevatorId = elevatorBId,
                DestinationFloor = 5
            });
            modelBuilder.Entity<Movement>().HasData(new Movement
            {
                ElevatorId = elevatorBId,
                DestinationFloor = 8
            });
        }

        public override Task<int> SaveChangesAsync(CancellationToken cancellationToken = new CancellationToken())
        {
            foreach (var entry in ChangeTracker.Entries<AuditableEntity>())
            {
                switch (entry.State)
                {
                    case EntityState.Added:
                        entry.Entity.CreatedDate = DateTime.Now;
                        entry.Entity.CreatedBy = _loggedInUserService.UserId;
                        break;
                    case EntityState.Modified:
                        entry.Entity.LastModifiedDate = DateTime.Now;
                        entry.Entity.LastModifiedBy = _loggedInUserService.UserId;
                        break;
                }
            }
            return base.SaveChangesAsync(cancellationToken);
        }
    }
}
