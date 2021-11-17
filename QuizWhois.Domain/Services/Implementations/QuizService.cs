using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.Extensions.Logging;
using QuizWhois.Common.Models;
using QuizWhois.Domain.Database;
using QuizWhois.Domain.Entity;
using QuizWhois.Domain.Services.Interfaces;

namespace QuizWhois.Domain.Services.Implementations
{
    public class QuizService : IQuizService
    {
        private readonly ApplicationContext _dbContext;
        private readonly ILogger<QuizService> _logger;

        public QuizService(ApplicationContext context, ILogger<QuizService> logger)
        {
            _dbContext = context;
            _logger = logger;
        }

        public async Task<QuizModel> CreateQuiz(List<long> questionIds, string quizName = "")
        {
            var quizToSave = new Quiz(quizName);
            string questionIdsToLog = string.Empty;
            foreach (var question in questionIds)
            {
                await AddQuestionToQuiz(quizToSave, question);
                questionIdsToLog += $"{question} ";
            }

            await _dbContext.AddAsync(quizToSave);
            await _dbContext.SaveChangesAsync();
            _logger.LogInformation($"Quiz id = {quizToSave.Id} was saved with question ids {questionIdsToLog}");
            var questions = quizToSave.Questions.Select(q => 
                new QuestionModel(q.Id, q.QuestionText, q.CorrectAnswers.Select(x => x.AnswerText).ToList()));
            return new QuizModel(quizToSave.Id, questions, quizToSave.Name);
        }

        public async Task AddToQuiz(AddToSetModel addToSetModel)
        {
            var quiz = await QuizById(addToSetModel.QuizId);
            foreach (var questionId in addToSetModel.QuestionIds)
            {
                await AddQuestionToQuiz(quiz, questionId);
            }

            this._dbContext.SaveChanges();

            string messageToLog = "Questions with id ";
            foreach (var questionId in addToSetModel.QuestionIds)
            {
                messageToLog += $"{questionId} ";
            }

            messageToLog += $"have been added to quiz with id {quiz.Id}";
            _logger.LogInformation(messageToLog);
        }

        public QuizModel GetQuiz(long quizId)
        {
            if (quizId <= 0)
            {
                throw new Exception("Id was invalid number");
            }

            var entity = _dbContext.Quizzes.Where(x => x.Id == quizId).Select(x => new
            {
                Id = x.Id,
                Name = x.Name,
                Questions = x.Questions.Select(y => new Question
                {
                    Id = y.Id,
                    QuizId = y.QuizId,
                    QuestionText = y.QuestionText,
                    CorrectAnswers = y.CorrectAnswers.ToList()
                }).ToList()
            }).FirstOrDefault();

            var questionModels = new List<QuestionModel>();
            entity.Questions.ForEach(x =>
            {
                var correctAnswers = new List<string>();
                var entityAnswers = _dbContext.Set<CorrectAnswer>().Where(y => y.QuestionId == x.Id);
                entityAnswers.ToList().ForEach(y => correctAnswers.Add(y.AnswerText));
                questionModels.Add(new QuestionModel(x.Id, x.QuestionText, correctAnswers));
            });
            return new QuizModel(entity.Id, questionModels, entity.Name);
        }

        private async Task<Quiz> QuizById(long quizId)
        {            
            var quiz = await _dbContext.Quizzes.FindAsync(quizId)
                ?? throw new System.ArgumentException($"Quiz with ID {quizId} does not exist.");
            
            return quiz;
        }

        private async Task AddQuestionToQuiz(Quiz quiz, long question)
        {
            var questionToAdd = await _dbContext.Questions.FindAsync(question);            
            if (questionToAdd != null && quiz != null && !quiz.Questions.Contains(questionToAdd))
            {
                quiz.Questions.Add(questionToAdd);
            }
            else if (questionToAdd == null)
            {
                throw new System.ArgumentException("Question with given ID does not exist.");
            }            
        }
    }
}
