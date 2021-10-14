using QuizWhois.Common.Models;
using QuizWhois.Domain.Database;
using QuizWhois.Domain.Entity;
using QuizWhois.Domain.Services.Interfaces;
using System;

namespace QuizWhois.Domain.Services.Implementations
{
    public class QuestionService : IQuestionService
    {
        public ApplicationContext _context { get; set; }

        public QuestionService(ApplicationContext context)
        {
            _context = context;
        }

        public QuestionModel AddQuestion(QuestionModel operationModel)
        {
            if (operationModel == null || operationModel.QuestionText == string.Empty || operationModel.CorrectAnswer == string.Empty)
            {
                throw new Exception("Operation Model was null");
            }
            var entity = new Question(operationModel.QuestionText, operationModel.CorrectAnswer);
            _context.Set<Question>().Add(entity);
            _context.SaveChanges();
            return new QuestionModel(entity.Id, entity.QuestionText, entity.CorrectAnswer);
        }

        
    }
}
