using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using QuizWhois.Domain.Entity;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace QuizWhois.Domain.Database
{
    public class ApplicationContext : DbContext
    {
        public DbSet<Question> Questions { get; set; }
        public DbSet<QuestionRating> QuestionRatings { get; set; }
        public DbSet<User> Users { get; set; }

        public ApplicationContext(DbContextOptions<ApplicationContext> options)
            : base(options)
        {
            Database.EnsureCreated();
        }

        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            modelBuilder.Entity<Question>(QuestionConfigure);
            modelBuilder.Entity<QuestionRating>(QuestionRatingConfigure);
            modelBuilder.Entity<User>(UserConfigure);
            modelBuilder.Entity<User>().HasData(
                new User { Id = 1, Login = "Qwerty" }
                );
        }

        public void QuestionConfigure(EntityTypeBuilder<Question> builder)
        {
            builder.ToTable("Question").HasKey(x => x.Id);
            builder.Property(x => x.QuestionText).IsRequired().HasMaxLength(255);
            builder.Property(x => x.CorrectAnswer).IsRequired().HasMaxLength(255);
        }

        public void QuestionRatingConfigure(EntityTypeBuilder<QuestionRating> builder)
        {
            builder.ToTable("QuestionRating").HasKey(x => x.Id);
            builder.Property(x => x.RatingNumber).IsRequired();
            builder.Property(x => x.QuestionId).IsRequired();
            builder.Property(x => x.UserId).IsRequired();
        }

        public void UserConfigure(EntityTypeBuilder<User> builder)
        {
            builder.ToTable("User").HasKey(x => x.Id);
            builder.Property(x => x.Login).IsRequired().HasMaxLength(30);
        }
    }
}
