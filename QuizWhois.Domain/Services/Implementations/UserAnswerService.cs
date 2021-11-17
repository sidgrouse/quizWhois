using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using QuizWhois.Common.Models;
using QuizWhois.Domain.Database;
using QuizWhois.Domain.Entity;
using QuizWhois.Domain.Services.Interfaces;

namespace QuizWhois.Domain.Services.Implementations
{
    public class UserAnswerService : IUserAnswerService
    {
        private readonly ApplicationContext _context;

        public UserAnswerService(ApplicationContext context)
        {
            _context = context;
        }

        public QuestionModel GetRandomQuestion()
        {
            var random = new Random().Next(0, _context.Set<Question>().Count());
            var randomRecord = _context.Set<Question>().OrderBy(x => x.Id).Skip(random).FirstOrDefault();
            if (randomRecord is null)
            {
                throw new ArgumentNullException("GetRandomQuestion in UserAnswerService.GetRandomQuestion was null");
            }
            else
            {
                return new QuestionModel(randomRecord.Id, randomRecord.QuestionText, randomRecord.CorrectAnswers
                    .Select(x => x.AnswerText).ToList());
            }
        }

        public bool CheckAnswer(UserAnswerModel userAnswerModel)
        {
            if (userAnswerModel == null || userAnswerModel.UserAnswerText == string.Empty)
            {
                throw new ArgumentNullException("UserAnswerModel in UserAnswerService.CheckAnswer was null");
            }

            var selectQuestion = _context.Set<Question>().Where(x => x.Id == userAnswerModel.Id).FirstOrDefault();
            if (selectQuestion is null)
            {
                throw new ArgumentNullException("selectQuestion in UserAnswerService.CheckAnswer was null");
            }
            else
            {
                return selectQuestion.CorrectAnswers.Select(x => x.AnswerText).Contains(userAnswerModel.UserAnswerText);
            }
        }
    }
}
