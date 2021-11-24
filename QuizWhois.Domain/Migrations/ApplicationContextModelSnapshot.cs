﻿// <auto-generated />
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Infrastructure;
using Microsoft.EntityFrameworkCore.Storage.ValueConversion;
using QuizWhois.Domain.Database;

namespace QuizWhois.Domain.Migrations
{
    [DbContext(typeof(ApplicationContext))]
    partial class ApplicationContextModelSnapshot : ModelSnapshot
    {
        protected override void BuildModel(ModelBuilder modelBuilder)
        {
#pragma warning disable 612, 618
            modelBuilder
                .HasAnnotation("Relational:MaxIdentifierLength", 64)
                .HasAnnotation("ProductVersion", "5.0.11");

            modelBuilder.Entity("QuizWhois.Domain.Entity.CorrectAnswer", b =>
                {
                    b.Property<long>("Id")
                        .ValueGeneratedOnAdd()
                        .HasColumnType("bigint");

                    b.Property<string>("AnswerText")
                        .IsRequired()
                        .HasColumnType("longtext");

                    b.Property<long>("QuestionId")
                        .HasColumnType("bigint");

                    b.HasKey("Id");

                    b.HasIndex("QuestionId");

                    b.ToTable("CorrectAnswer");
                });

            modelBuilder.Entity("QuizWhois.Domain.Entity.Hint", b =>
                {
                    b.Property<long>("Id")
                        .ValueGeneratedOnAdd()
                        .HasColumnType("bigint");

                    b.Property<long>("QuestionId")
                        .HasColumnType("bigint");

                    b.Property<string>("Text")
                        .IsRequired()
                        .HasMaxLength(255)
                        .HasColumnType("varchar(255)");

                    b.HasKey("Id");

                    b.HasIndex("QuestionId");

                    b.ToTable("Hint");
                });

            modelBuilder.Entity("QuizWhois.Domain.Entity.Pack", b =>
                {
                    b.Property<long>("Id")
                        .ValueGeneratedOnAdd()
                        .HasColumnType("bigint");

                    b.Property<string>("Description")
                        .HasColumnType("longtext");

                    b.Property<bool>("IsDraft")
                        .HasColumnType("tinyint(1)");

                    b.Property<string>("Name")
                        .HasColumnType("longtext");

                    b.HasKey("Id");

                    b.ToTable("Packs");
                });

            modelBuilder.Entity("QuizWhois.Domain.Entity.Question", b =>
                {
                    b.Property<long>("Id")
                        .ValueGeneratedOnAdd()
                        .HasColumnType("bigint");

                    b.Property<long>("PackId")
                        .HasColumnType("bigint");

                    b.Property<string>("QuestionText")
                        .IsRequired()
                        .HasMaxLength(255)
                        .HasColumnType("varchar(255)");

                    b.HasKey("Id");

                    b.HasIndex("PackId");

                    b.ToTable("Question");
                });

            modelBuilder.Entity("QuizWhois.Domain.Entity.QuestionRating", b =>
                {
                    b.Property<long>("Id")
                        .ValueGeneratedOnAdd()
                        .HasColumnType("bigint");

                    b.Property<long>("QuestionId")
                        .HasColumnType("bigint");

                    b.Property<long>("UserId")
                        .HasColumnType("bigint");

                    b.Property<uint>("Value")
                        .HasColumnType("int unsigned");

                    b.HasKey("Id");

                    b.HasIndex("QuestionId");

                    b.HasIndex("UserId");

                    b.ToTable("QuestionRating");
                });

            modelBuilder.Entity("QuizWhois.Domain.Entity.User", b =>
                {
                    b.Property<long>("Id")
                        .ValueGeneratedOnAdd()
                        .HasColumnType("bigint");

                    b.Property<string>("Login")
                        .IsRequired()
                        .HasMaxLength(30)
                        .HasColumnType("varchar(30)");

                    b.HasKey("Id");

                    b.ToTable("User");

                    b.HasData(
                        new
                        {
                            Id = 1L,
                            Login = "Qwerty"
                        },
                        new
                        {
                            Id = 2L,
                            Login = "Asdfg"
                        });
                });

            modelBuilder.Entity("QuizWhois.Domain.Entity.CorrectAnswer", b =>
                {
                    b.HasOne("QuizWhois.Domain.Entity.Question", "Question")
                        .WithMany("CorrectAnswers")
                        .HasForeignKey("QuestionId")
                        .OnDelete(DeleteBehavior.Cascade)
                        .IsRequired();

                    b.Navigation("Question");
                });

            modelBuilder.Entity("QuizWhois.Domain.Entity.Hint", b =>
                {
                    b.HasOne("QuizWhois.Domain.Entity.Question", "Question")
                        .WithMany("Hints")
                        .HasForeignKey("QuestionId")
                        .OnDelete(DeleteBehavior.Cascade)
                        .IsRequired();

                    b.Navigation("Question");
                });

            modelBuilder.Entity("QuizWhois.Domain.Entity.Question", b =>
                {
                    b.HasOne("QuizWhois.Domain.Entity.Pack", null)
                        .WithMany("Questions")
                        .HasForeignKey("PackId")
                        .OnDelete(DeleteBehavior.Cascade)
                        .IsRequired();
                });

            modelBuilder.Entity("QuizWhois.Domain.Entity.QuestionRating", b =>
                {
                    b.HasOne("QuizWhois.Domain.Entity.Question", "Question")
                        .WithMany()
                        .HasForeignKey("QuestionId")
                        .OnDelete(DeleteBehavior.Cascade)
                        .IsRequired();

                    b.HasOne("QuizWhois.Domain.Entity.User", "User")
                        .WithMany()
                        .HasForeignKey("UserId")
                        .OnDelete(DeleteBehavior.Cascade)
                        .IsRequired();

                    b.Navigation("Question");

                    b.Navigation("User");
                });

            modelBuilder.Entity("QuizWhois.Domain.Entity.Pack", b =>
                {
                    b.Navigation("Questions");
                });

            modelBuilder.Entity("QuizWhois.Domain.Entity.Question", b =>
                {
                    b.Navigation("CorrectAnswers");

                    b.Navigation("Hints");
                });
#pragma warning restore 612, 618
        }
    }
}
