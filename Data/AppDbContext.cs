using Microsoft.EntityFrameworkCore;
using QuizForge.Desktop.Models;

namespace QuizForge.Desktop.Data
{
    public class AppDbContext : DbContext
    {
        public DbSet<User> Users => Set<User>();
        public DbSet<Category> Categories => Set<Category>();
        public DbSet<Question> Questions => Set<Question>();
        public DbSet<QuizOption> QuizOptions => Set<QuizOption>();
        public DbSet<QuizAttempt> QuizAttempts => Set<QuizAttempt>();
        public DbSet<AttemptAnswer> AttemptAnswers => Set<AttemptAnswer>();
        public DbSet<QuizHistory> QuizHistory => Set<QuizHistory>();

        protected override void OnConfiguring(
            DbContextOptionsBuilder optionsBuilder)
        {
            const string connectionString =
                "Server=localhost;" +
                "Port=3306;" +
                "Database=quizforge;" +
                "User=root;" +
                "Password=Atul7990#;";

            optionsBuilder.UseMySql(
                connectionString,
                ServerVersion.AutoDetect(connectionString));
        }

        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            base.OnModelCreating(modelBuilder);

            modelBuilder.Entity<User>()
                .HasKey(u => u.UserId);

            modelBuilder.Entity<Category>()
                .HasKey(c => c.CategoryId);

            modelBuilder.Entity<Question>()
                .HasKey(q => q.QuestionId);

            modelBuilder.Entity<QuizOption>()
                .HasKey(o => o.OptionId);

            modelBuilder.Entity<QuizAttempt>()
                .HasKey(a => a.AttemptId);

            modelBuilder.Entity<AttemptAnswer>()
                .HasKey(a => a.AttemptAnswerId);

            modelBuilder.Entity<QuizHistory>()
                .HasKey(h => h.HistoryId);
        }
    }
}