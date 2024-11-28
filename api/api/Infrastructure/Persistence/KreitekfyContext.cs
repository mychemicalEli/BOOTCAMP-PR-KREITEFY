using api.Domain.Entities;
using Microsoft.EntityFrameworkCore;

namespace api.Infrastructure.Persistence;

public class KreitekfyContext : DbContext
{
    public KreitekfyContext(DbContextOptions<KreitekfyContext> options) : base(options)
    {
    }

    protected override void OnConfiguring(DbContextOptionsBuilder _optionsBuilder)
    {
        base.OnConfiguring(_optionsBuilder);
    }

    protected override void OnModelCreating(ModelBuilder modelBuilder)
    {
        modelBuilder.Entity<User>()
            .HasOne(i => i.Role)
            .WithMany()
            .HasForeignKey(i => i.RoleId)
            .IsRequired();

        modelBuilder.Entity<Song>()
            .HasOne(i => i.Artist)
            .WithMany()
            .HasForeignKey(i => i.ArtistId)
            .IsRequired();

        modelBuilder.Entity<Song>()
            .HasOne(i => i.Album)
            .WithMany()
            .HasForeignKey(i => i.AlbumId)
            .IsRequired()
            .OnDelete(DeleteBehavior.Cascade);

        modelBuilder.Entity<Song>()
            .HasOne(i => i.Genre)
            .WithMany()
            .HasForeignKey(i => i.GenreId)
            .IsRequired();


        modelBuilder.Entity<UserSongs>()
            .HasOne<User>(i => i.User)
            .WithOne(u => u.UserSongs)
            .HasForeignKey<UserSongs>(us => us.UserId)
            .OnDelete(DeleteBehavior.Cascade);

        modelBuilder.Entity<UserSongs>()
            .HasOne<Song>(i => i.Song)
            .WithMany(us => us.UserSongs)
            .HasForeignKey(us => us.SongId)
            .OnDelete(DeleteBehavior.Cascade);

        modelBuilder.Entity<Rating>()
            .HasOne<User>(i => i.User)
            .WithMany(r => r.Ratings)
            .HasForeignKey(r => r.UserId)
            .OnDelete(DeleteBehavior.Cascade);

        modelBuilder.Entity<Rating>()
            .HasOne<Song>(i => i.Song)
            .WithMany(r => r.Ratings)
            .HasForeignKey(r => r.SongId)
            .OnDelete(DeleteBehavior.Cascade);
    }

    public DbSet<Role> Roles { get; set; }
    public DbSet<User> Users { get; set; }
    public DbSet<Genre> Genres { get; set; }
    public DbSet<Artist> Artists { get; set; }
    public DbSet<Album> Albums { get; set; }
    public DbSet<Song> Songs { get; set; }
    public DbSet<UserSongs> UserSongsHistories { get; set; }
    public DbSet<Rating> Ratings { get; set; }
}