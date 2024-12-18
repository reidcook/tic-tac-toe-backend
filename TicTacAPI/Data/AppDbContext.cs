using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
using TicTacAPI.SignalModels;

namespace TicTacAPI.Data
{
    public class AppDbContext : DbContext
    {
        protected readonly IConfiguration _configuration;
        public AppDbContext(IConfiguration configuration)
        {
            _configuration = configuration;
        }
        protected override void OnConfiguring(DbContextOptionsBuilder options)
        {
            options.UseNpgsql(_configuration.GetConnectionString("WebApiDatabase"));
        }
        public DbSet<Game> Games { get; set; }
        public DbSet<UserConnection> Connections { get; set; }
    }
}
