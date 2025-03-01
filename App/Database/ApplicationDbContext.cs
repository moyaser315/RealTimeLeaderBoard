using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.EntityFrameworkCore;

namespace App.Database
{
    public class ApplicationDbContext(DbContextOptions<ApplicationDbContext> options) : DbContext(options)
    {

        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            modelBuilder.Entity<UserModel>()
            .HasIndex(u => u.UserName)
            .IsUnique();

            modelBuilder.Entity<UserModel>()
            .HasIndex(u => u.Email)
            .IsUnique();
            
            modelBuilder.Entity<ScoreModel>()
            .Property(s => s.TimeStamp)
            .HasDefaultValueSql("NOW()");

            // User - Score (O - M)
            modelBuilder.Entity<ScoreModel>()
            .HasOne(s => s.User)
            .WithMany(u => u.Scores)
            .HasForeignKey(s => s.UserId)
            .OnDelete(DeleteBehavior.Cascade);

            // Game - Score (O - M)
            modelBuilder.Entity<ScoreModel>()
            .HasOne(s => s.Game)
            .WithMany(u => u.Scores)
            .HasForeignKey(s => s.GameID)
            .OnDelete(DeleteBehavior.Cascade);
        }

        public DbSet<UserModel> Users { get; set; }
        public DbSet<ScoreModel> Scores { get; set; }
        public DbSet<GameModel> Games { get; set; }
    }
}