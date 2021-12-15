using System;
using System.Linq;
using AutoMapper;
using QuizWhois.Common.Models;
using QuizWhois.Domain.Database;
using QuizWhois.Domain.Entity;
using QuizWhois.Domain.Services.Interfaces;
using QuizWhois.Domain.Services.Mapper;

namespace QuizWhois.Domain.Services.Implementations
{
    public class UserAnswerService : IUserAnswerService
    {
        private readonly ApplicationContext _context;
        private readonly IMapper _mapper;

        public UserAnswerService(ApplicationContext context, IMapper mapper)
        {
            _context = context;
            _mapper = mapper;
        }

        public QuestionModelResponse GetRandomQuestion()
        {
            var random = new Random().Next(0, _context.Set<Question>().Count());
            var randomRecord = _context.Set<Question>().OrderBy(x => x.Id).Skip(random).FirstOrDefault();
            if (randomRecord is null)
            {
                throw new ArgumentNullException("GetRandomQuestion in UserAnswerService.GetRandomQuestion was null");
            }
            else
            {
                return randomRecord.ToQuestionModelResponse();
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
