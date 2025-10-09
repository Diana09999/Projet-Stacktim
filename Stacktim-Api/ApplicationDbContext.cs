using Microsoft.EntityFrameworkCore;

public class ApplicationDbContext : DbContext
{
    public ApplicationDbContext(DbContextOptions<ApplicationDbContext> options)
        : base(options)
    {}
    public DbSet<Player> Players { get; set; }
    public DbSet<Team> Teams { get; set; }
    public DbSet<TeamPlayer> TeamPlayers { get; set; } 

    protected override void OnModelCreating(ModelBuilder modelBuilder)
    {
        base.onModelCreating(modelBuilder);

        modelBuilder.Entity<TeamPlayers>()
            .HasKey(tp => new {tp.TeamId, tp.PlayerId});
    }
}