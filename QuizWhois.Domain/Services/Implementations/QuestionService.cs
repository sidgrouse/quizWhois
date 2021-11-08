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
    public class QuestionService : IQuestionService
    {
        private readonly ApplicationContext _context;
        private readonly ILogger<QuestionService> _logger;

        public QuestionService(ApplicationContext context, ILogger<QuestionService> logger)
        {
            _context = context;
            _logger = logger;
        }

        public async Task<QuestionModel> AddQuestion(QuestionModel questionModel)
        {
            if (questionModel == null || questionModel.QuestionText == string.Empty || questionModel.CorrectAnswers.Count == 0)
            {
                throw new Exception("Operation Model was null");
            }

            var correctAnswers = new List<CorrectAnswer>();
            questionModel.CorrectAnswers.ForEach(x => correctAnswers.Add(new CorrectAnswer(x)));
            var entity = new Question { QuestionText = questionModel.QuestionText, CorrectAnswers = correctAnswers };
            var result = await _context.Questions.AddAsync(entity);
            await _context.SaveChangesAsync();
            _logger.LogInformation($"Question id = {entity.Id} was added");
            var correctAnswersToModel = new List<string>();
            result.Entity.CorrectAnswers.ForEach(x => correctAnswersToModel.Add(x.AnswerText));
            return new QuestionModel(result.Entity.Id, result.Entity.QuestionText, correctAnswersToModel);
        }

        public QuestionModel GetQuestion(long questionId)
        {
            if (questionId <= 0)
            {
                throw new Exception("Id was invalid number");
            }

            var entity = _context.Questions.Select(x => new
                {
                    id = x.Id,
                    questionText = x.QuestionText,
                    correctAnswers = x.CorrectAnswers
                }).FirstOrDefault(x => x.id == questionId);

            var correctAnswers = new List<string>();
            entity.correctAnswers.ToList().ForEach(x => correctAnswers.Add(x.AnswerText));
            return new QuestionModel(entity.id, entity.questionText, correctAnswers);
        }

        public async Task CreateQuestions(IEnumerable<QuestionModel> questionsToAdd)
        {
            foreach (var question in questionsToAdd)
            {
               await AddQuestion(question);
            }

            _context.SaveChanges();
        }
    }
}
