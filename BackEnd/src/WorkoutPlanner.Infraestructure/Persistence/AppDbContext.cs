using Microsoft.EntityFrameworkCore;
using WorkoutPlanner.Domain;

namespace WorkoutPlanner.Infraestructure.Persistence;

public class AppDbContext : DbContext
{
    public AppDbContext(DbContextOptions<AppDbContext> options) : base(options){}

    public DbSet<User> Users => Set<User>();
    public DbSet<Excercise> Excercises => Set<Excercise>();
    public DbSet<Workout> Workouts => Set<Workout>();
    public DbSet<WorkoutExcercise> WorkoutExcercises => Set<WorkoutExcercise>();
    public DbSet<Evaluation> Evaluations => Set<Evaluation>();
    
    protected override void OnModelCreating(ModelBuilder modelBuilder)
    {
        base.OnModelCreating(modelBuilder);
        
        // User Configuration
        modelBuilder.Entity<User>(entity =>
        {
            entity.HasKey(u => u.Id);
            entity.Property(u => u.Role).HasConversion<string>();
            entity.HasIndex(u => u.Email).IsUnique();
            entity.HasIndex(u => u.Name).IsUnique();
            entity.Property(u => u.Email).IsRequired().HasMaxLength(100);
            entity.Property(u => u.Name).IsRequired().HasMaxLength(50);
            entity.Property(u => u.PasswordHash).IsRequired();
        });
        
        // Workout Configuration
        modelBuilder.Entity<Workout>(entity =>
        {
            entity.HasKey(w => w.Id);
            entity.HasIndex(w => w.Name).IsUnique();
            entity.Property(w => w.Name).IsRequired().HasMaxLength(100);

            entity.HasOne(w => w.Coach)
                .WithMany()
                .HasForeignKey(w => w.CoachId)
                .OnDelete(DeleteBehavior.Restrict); 
        });
        
        // WorkoutExcercise Configuration
        modelBuilder.Entity<WorkoutExcercise>(entity =>
        {
            entity.HasKey(we => we.Id);
            entity.Property(we => we.LoadType).HasConversion<string>();

            entity.HasOne(we => we.Workout)
                .WithMany(w => w.WorkoutExcercises)
                .HasForeignKey(we => we.WorkoutId);

            entity.HasOne(we => we.Excercise)
                .WithMany()
                .HasForeignKey(we => we.ExcerciseId);
        });
        
        // Evaluation Configuration
        modelBuilder.Entity<Evaluation>(entity =>
        {
            entity.HasKey(ev => ev.Id);

            entity.HasOne(ev => ev.Player)
                .WithMany()
                .HasForeignKey(ev => ev.PlayerId)
                .OnDelete(DeleteBehavior.Cascade);

            entity.HasOne(ev => ev.Excercise)
                .WithMany()
                .HasForeignKey(ev => ev.ExcerciseId)
                .OnDelete(DeleteBehavior.Restrict);
        });
    }
}