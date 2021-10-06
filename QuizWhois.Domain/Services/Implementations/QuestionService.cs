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

        public QuestionModel GetRandomQuestion()
        {
            var random = new Random().Next(0, _context.Set<Question>().Count());
            var randomRecord = _context.Set<Question>().OrderBy(x => x.Id).Skip(random).FirstOrDefault();
            if (randomRecord is null)
            {
                throw new Exception("GetRandomQuestion in QuestionService.GetRandomQuestion was null");
            }
            else
                return new QuestionModel(randomRecord.Id, randomRecord.QuestionText, randomRecord.CorrectAnswer);
        }

        public bool CheckAnswer(QuestionModel operationModel)
        {
            if (operationModel == null || operationModel.QuestionText == string.Empty || operationModel.CorrectAnswer == string.Empty)
            {
                throw new Exception("OperationModel in QuestionService.CheckAnswer was null");
            }
            var selectQuestion = _context.Set<Question>().Where(x => x.Id == operationModel.Id && x.QuestionText == operationModel.QuestionText).FirstOrDefault();
            if (selectQuestion is null)
            {
                throw new Exception("selectQuestion in QuestionService.CheckAnswer was null");
            }
            else
                return operationModel.CorrectAnswer.Equals(selectQuestion.CorrectAnswer);

        }
    }
}
