using QuizWhois.Common.Models;
using QuizWhois.Domain.Database;
using QuizWhois.Domain.Entity;
using QuizWhois.Domain.Services.Interfaces;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace QuizWhois.Domain.Services.Implementations
{
    public class QuestionService : IQuestionService
    {
        public ApplicationContext _context { get; set; }

        public QuestionService(ApplicationContext context)
        {
            _context = context;
        }

        public void AddQuestion(QuestionModel operationModel)
        {
            if (operationModel == null || operationModel.QuestionText == string.Empty || operationModel.CorrectAnswer == string.Empty)
            {
                throw new Exception("Operation Model was null");
            }
            _context.Set<Question>().Add(new Question(operationModel.Id, operationModel.QuestionText, operationModel.CorrectAnswer));
            _context.SaveChanges();
        }
    }
}
