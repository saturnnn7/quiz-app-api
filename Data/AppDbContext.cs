using Microsoft.EntityFrameworkCore;
using quiz.app.api.Models;

namespace quiz.app.api.Data;

public class AppDbContext : DbContext, IAppDbContext
{
    public AppDbContext(DbContextOptions<AppDbContext> options) : base(options) { }

    public DbSet<User>           Users     => Set<User>();
    public DbSet<Quiz>           Quizzes   => Set<Quiz>();
    public DbSet<Question>       Questions => Set<Question>();
    public DbSet<ResultEntity>   Results => Set<ResultEntity>();

    protected override void OnModelCreating(ModelBuilder modelBuilder)
    {
        base.OnModelCreating(modelBuilder);

        // User
        modelBuilder.Entity<User>(e =>
        {
            e.HasKey(u => u.Id);
            e.HasIndex(u => u.Email).IsUnique();
            e.HasIndex(u => u.Username).IsUnique();
            e.Property(u => u.Username).HasMaxLength(50).IsRequired();
            e.Property(u => u.Email).HasMaxLength(256).IsRequired();
            e.Property(u => u.PasswordHash).IsRequired();
        });

        // Quiz
        modelBuilder.Entity<Quiz>(e =>
        {
            e.HasKey(q => q.Id);
            e.Property(q => q.Title).HasMaxLength(200).IsRequired();
            e.Property(q => q.Description).HasMaxLength(1000);
            e.HasOne(q => q.Author)
             .WithMany(u => u.Quizzes)
             .HasForeignKey(q => q.AuthorId)
             .OnDelete(DeleteBehavior.Cascade);
        });

        // Question
        modelBuilder.Entity<Question>(e =>
        {
            e.HasKey(q => q.Id);
            e.Property(q => q.Text).HasMaxLength(500).IsRequired();
            e.Property(q => q.AnswerA).HasMaxLength(200).IsRequired();
            e.Property(q => q.AnswerB).HasMaxLength(200).IsRequired();
            e.Property(q => q.AnswerC).HasMaxLength(200).IsRequired();
            e.Property(q => q.AnswerD).HasMaxLength(200).IsRequired();
            e.HasOne(q => q.Quiz)
             .WithMany(q => q.Questions)
             .HasForeignKey(q => q.QuizId)
             .OnDelete(DeleteBehavior.Cascade);
        });

        // Result
        modelBuilder.Entity<Models.ResultEntity>(e =>
        {
            e.HasKey(r => r.Id);
            e.HasOne(r => r.User)
             .WithMany(u => u.Results)
             .HasForeignKey(r => r.UserId)
             .OnDelete(DeleteBehavior.Cascade);
            e.HasOne(r => r.Quiz)
             .WithMany(q => q.Results)
             .HasForeignKey(r => r.QuizId)
             .OnDelete(DeleteBehavior.Cascade);
        });
    }
}