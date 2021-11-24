using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using QuizWhois.Domain.Entity;

namespace QuizWhois.Domain.Database
{
    public class ApplicationContext : DbContext
    {
        public DbSet<Question> Questions { get; set; }

        public DbSet<CorrectAnswer> CorrectAnswers { get; set; }

        public DbSet<QuestionRating> QuestionRatings { get; set; }

        public DbSet<User> Users { get; set; }

        public DbSet<Pack> Packs { get; set; }

        public DbSet<Hint> Hints { get; set; }

        public DbSet<QuestionImage> QuestionImages { get; set; }

        public ApplicationContext(DbContextOptions<ApplicationContext> options)
            : base(options)
        {
            Database.Migrate();
        }

        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            modelBuilder.Entity<Question>(QuestionConfigure);
            modelBuilder.Entity<QuestionRating>(QuestionRatingConfigure);
            modelBuilder.Entity<CorrectAnswer>(CorrectAnswerConfigure);
            modelBuilder.Entity<User>(UserConfigure);
            modelBuilder.Entity<User>().HasData(
                new User[]
                {
                    new User { Id = 1, Login = "Qwerty" },
                    new User { Id = 2, Login = "Asdfg" },
                });
            modelBuilder.Entity<Pack>().HasKey(q => q.Id);
            modelBuilder.Entity<Pack>().HasMany(x => x.Questions).WithOne();
            modelBuilder.Entity<Hint>(HintConfigure);
        }

        public void QuestionConfigure(EntityTypeBuilder<Question> builder)
        {
            builder.ToTable("Question").HasKey(x => x.Id);
            builder.HasOne(x => x.QuestionImage).WithOne(q => q.Question).HasForeignKey<QuestionImage>(i => i.QuestionId);
            builder.Property(x => x.QuestionText).IsRequired().HasMaxLength(255);            
        }

        public void QuestionRatingConfigure(EntityTypeBuilder<QuestionRating> builder)
        {
            builder.ToTable("QuestionRating").HasKey(x => x.Id);
            builder.Property(x => x.Value).IsRequired();
            builder.Property(x => x.QuestionId).IsRequired();
            builder.Property(x => x.UserId).IsRequired();
        }

        public void CorrectAnswerConfigure(EntityTypeBuilder<CorrectAnswer> builder)
        {
            builder.ToTable("CorrectAnswer").HasKey(x => x.Id);
            builder.Property(x => x.AnswerText).IsRequired();
            builder.Property(x => x.QuestionId).IsRequired();
        }

        public void UserConfigure(EntityTypeBuilder<User> builder)
        {
            builder.ToTable("User").HasKey(x => x.Id);
            builder.Property(x => x.Login).IsRequired().HasMaxLength(30);
        }

        public void HintConfigure(EntityTypeBuilder<Hint> builder)
        {
            builder.ToTable("Hint").HasKey(x => x.Id);
            builder.Property(x => x.Text).IsRequired().HasMaxLength(255);
            builder.HasOne(p => p.Question).WithMany(t => t.Hints);
        }
    }
}
