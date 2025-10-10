using System;
using System.Collections.Generic;
using Microsoft.EntityFrameworkCore;
using Stacktim.Models;

namespace Stacktim.Data;

public partial class StacktimContext : DbContext
{
    public StacktimContext()
    {
    }

    public StacktimContext(DbContextOptions<StacktimContext> options)
        : base(options)
    {
    }

    public virtual DbSet<Player> Players { get; set; }

    public virtual DbSet<Team> Teams { get; set; }

    public virtual DbSet<TeamPlayer> TeamPlayers { get; set; }

    protected override void OnConfiguring(DbContextOptionsBuilder optionsBuilder)
#warning To protect potentially sensitive information in your connection string, you should move it out of source code. You can avoid scaffolding the connection string by using the Name= syntax to read it from configuration - see https://go.microsoft.com/fwlink/?linkid=2131148. For more guidance on storing connection strings, see https://go.microsoft.com/fwlink/?LinkId=723263.
        => optionsBuilder.UseSqlServer("Server=ACASRVRDS001;Database=Stacktim;Trusted_Connection=True;TrustServerCertificate=True;");

    protected override void OnModelCreating(ModelBuilder modelBuilder)
    {
        modelBuilder.Entity<Player>(entity =>
        {
            entity.HasKey(e => e.IdPlayers).HasName("PK__Players__8FEDD67C98B627B5");

            entity.HasIndex(e => e.Name, "UQ__Players__737584F660C1C0F6").IsUnique();

            entity.HasIndex(e => e.Email, "UQ__Players__A9D1053493E6CFD3").IsUnique();

            entity.Property(e => e.IdPlayers).HasColumnName("Id_Players");
            entity.Property(e => e.Email)
                .HasMaxLength(100)
                .IsUnicode(false);
            entity.Property(e => e.Name)
                .HasMaxLength(50)
                .IsUnicode(false);
            entity.Property(e => e.RankPlayer)
                .HasMaxLength(20)
                .IsUnicode(false);
            entity.Property(e => e.RegistrationDate).HasDefaultValueSql("(getdate())");
        });

        modelBuilder.Entity<Team>(entity =>
        {
            entity.HasKey(e => e.IdTeams).HasName("PK__Teams__BA144B3552FAF2D4");

            entity.HasIndex(e => e.Name, "UQ__Teams__737584F62FD82B32").IsUnique();

            entity.HasIndex(e => e.Tag, "UQ__Teams__C4516413FE09DDBE").IsUnique();

            entity.Property(e => e.IdTeams).HasColumnName("Id_Teams");
            entity.Property(e => e.CreationDate).HasDefaultValueSql("(getdate())");
            entity.Property(e => e.Name)
                .HasMaxLength(50)
                .IsUnicode(false);
            entity.Property(e => e.Tag)
                .HasMaxLength(3)
                .IsUnicode(false)
                .IsFixedLength();

            entity.HasOne(d => d.Captain).WithMany(p => p.Teams)
                .HasForeignKey(d => d.CaptainId)
                .OnDelete(DeleteBehavior.ClientSetNull)
                .HasConstraintName("FK_Teams_Captain");
        });

        modelBuilder.Entity<TeamPlayer>(entity =>
        {
            entity.HasKey(e => new { e.PlayerId, e.TeamId }).HasName("PK__TeamPlay__CB6DDAB19D2234FF");

            entity.Property(e => e.JoinDate)
                .HasDefaultValueSql("(getdate())")
                .HasColumnType("datetime");

            entity.HasOne(d => d.Player).WithMany(p => p.TeamPlayers)
                .HasForeignKey(d => d.PlayerId)
                .HasConstraintName("FK__TeamPlaye__Playe__32AB8735");

            entity.HasOne(d => d.Team).WithMany(p => p.TeamPlayers)
                .HasForeignKey(d => d.TeamId)
                .HasConstraintName("FK__TeamPlaye__TeamI__339FAB6E");
        });

        OnModelCreatingPartial(modelBuilder);
    }

    partial void OnModelCreatingPartial(ModelBuilder modelBuilder);
}
