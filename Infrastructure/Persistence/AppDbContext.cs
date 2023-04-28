
using Domain.Entities;
using Microsoft.EntityFrameworkCore;

namespace Infrastructure.Persistence
{
    public class AppDbContext : DbContext
    {
        public AppDbContext(DbContextOptions<AppDbContext> options) : base(options) { }

        public DbSet<Match> Matches { get; set; }
        public DbSet<UserMatch> UserMatches { get; set; }
        public DbSet<Date> Dates { get; set; }

        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            modelBuilder.Entity<Match>(entity =>
            {
                entity.HasKey(m => m.MatchId);
                entity.Property(m => m.MatchId).ValueGeneratedOnAdd();
                entity.Property(m => m.User1Id).HasColumnName("User1").IsRequired();
                entity.Property(m => m.User2Id).HasColumnName("User2").IsRequired();
                entity.Property(m => m.CreatedAt).ValueGeneratedOnAdd();
            });

            modelBuilder.Entity<Date>(entity =>
            {
                entity.HasKey(d => d.DateId);
                entity.Property(d => d.DateId).ValueGeneratedOnAdd();
                entity.Property(d => d.Description).HasMaxLength(255);
                entity.Property(d => d.State).HasDefaultValue(0);

            });

            modelBuilder.Entity<UserMatch>(entity =>
            {
                entity.HasKey(ue => ue.UserMatchId);
                entity.Property(ue => ue.UserMatchId).ValueGeneratedOnAdd();
                entity.Property(ue => ue.UserMainId).IsRequired();
                entity.Property(ue => ue.UserSecundaryId).IsRequired();
                entity.Property(ue => ue.CreatedAt).ValueGeneratedOnAdd();
                entity.Property(ue => ue.Like).IsRequired();
            });
        }
    }
}
