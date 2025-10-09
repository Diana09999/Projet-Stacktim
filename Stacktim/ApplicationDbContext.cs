using Microsoft.EntityFrameworkCore;

public class ApplicationDbContext : DbContext
{
    public ApplicationDbContext(DbContextOptions<ApplicationDbContext> options)
        : base(options)
    {}
    public DbSet<Player> Players { get; set; }
    public DbSet<Teams> Teams { get; set; }
    public DbSet<TeamPlayers> TeamPlayers { get; set; } 
}