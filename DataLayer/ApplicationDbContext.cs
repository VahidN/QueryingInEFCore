using EFCorePgExercises.Entities;
using EFCorePgExercises.Utils;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Logging;

namespace EFCorePgExercises.DataLayer
{
    public class ApplicationDbContext : DbContext
    {
        public ApplicationDbContext(DbContextOptions options)
            : base(options)
        {
        }

        public DbSet<Member> Members { get; set; }

        public DbSet<Booking> Bookings { get; set; }

        public DbSet<Facility> Facilities { get; set; }

        protected override void OnConfiguring(DbContextOptionsBuilder optionsBuilder)
        {
            optionsBuilder
                .UseLoggerFactory(LoggerFactory.Create(x => x
                    .AddConsole()
                    .AddFilter(y => y >= LogLevel.Debug)))
                .EnableSensitiveDataLogging()
                .EnableDetailedErrors();

            base.OnConfiguring(optionsBuilder);
        }

        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            base.OnModelCreating(modelBuilder);

            modelBuilder.AddCustomSqlFunctions();

            modelBuilder.ApplyConfigurationsFromAssembly(typeof(MemberConfiguration).Assembly);
        }
    }
}