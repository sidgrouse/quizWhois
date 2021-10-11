using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using QuizWhois.Domain.Entity;

namespace QuizWhois.Domain.Database
{
    public class ApplicationContext : DbContext
    {
        public DbSet<Question> Questions { get; set; }

        public DbSet<Quiz> Quizzes { get; set; }

        public ApplicationContext(DbContextOptions<ApplicationContext> options)
            : base(options)
        {
            Database.EnsureCreated();
        }

        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            modelBuilder.Entity<Question>().HasKey(q => q.Id);

            modelBuilder.Entity<Quiz>().HasKey(q => q.Id);
            modelBuilder.Entity<Quiz>().HasMany(x => x.Questions).WithOne();

        }
    }
}
