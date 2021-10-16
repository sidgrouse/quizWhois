using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using QuizWhois.Common.Models;
using QuizWhois.Domain.Database;
using QuizWhois.Domain.Entity;
using QuizWhois.Domain.Services.Interfaces;

namespace QuizWhois.Domain.Services.Implementations
{
    public class QuestionService : IQuestionService
    {
        private readonly ApplicationContext _context;

        public QuestionService(ApplicationContext context)
        {
            _context = context;
        }

        public async Task<QuestionModel> AddQuestion(QuestionModel operationModel)
        {
            if (operationModel == null || operationModel.QuestionText == string.Empty || operationModel.CorrectAnswer == string.Empty)
            {
                throw new Exception("Operation Model was null");
            }

            var entity = new Question(operationModel.QuestionText, operationModel.CorrectAnswer);
            await _context.Set<Question>().AddAsync(entity);
            await _context.SaveChangesAsync();
            return new QuestionModel(entity.Id, entity.QuestionText, entity.CorrectAnswer);
        }

        public async Task AddMany(IEnumerable<QuestionModel> questionsToAdd)
        {
            foreach (var question in questionsToAdd)
            {
               await AddQuestion(question);
            }

            _context.SaveChanges();
        }
    }
}
