using Microsoft.AspNetCore.Identity.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore;
using RoutesGeneratorWithMicroServices.Models;

namespace RoutesGeneratorWithMicroServices.Data
{
    public class ApplicationDbContext : IdentityDbContext
    {
        public ApplicationDbContext(DbContextOptions<ApplicationDbContext> options)
            : base(options)
        {
        }
        public DbSet<RoutesGeneratorWithMicroServices.Models.City> City { get; set; }
        public DbSet<RoutesGeneratorWithMicroServices.Models.Person> Person { get; set; }
        public DbSet<RoutesGeneratorWithMicroServices.Models.Team> Team { get; set; }
        public DbSet<RoutesGeneratorWithMicroServices.Models.FileReceiver> FileReceiver { get; set; }
    }
}
