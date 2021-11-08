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

        public async Task<QuestionModel> AddQuestion(QuestionModel operationModel)
        {
            if (operationModel == null || operationModel.QuestionText == string.Empty || operationModel.CorrectAnswers.Count == 0)
            {
                throw new Exception("Operation Model was null");
            }

            var entity = new Question(operationModel.QuestionText, operationModel.CorrectAnswers);
            await _context.Questions.AddAsync(entity);
            await _context.SaveChangesAsync();
            _logger.LogInformation($"Question id = {entity.Id} was added");
            return new QuestionModel(entity.Id, entity.QuestionText, entity.CorrectAnswers.Select(x => x.AnswerText).ToList());
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
