using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using AutoMapper;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Logging;
using QuizWhois.Common;
using QuizWhois.Common.Models;
using QuizWhois.Domain.Database;
using QuizWhois.Domain.Entity;
using QuizWhois.Domain.Services.Interfaces;
using QuizWhois.Domain.Services.Mapper;

namespace QuizWhois.Domain.Services.Implementations
{
    public class QuestionService : IQuestionService
    {
        private readonly ApplicationContext _context;
        private readonly ILogger<QuestionService> _logger;
        private readonly IMapper _mapper;

        public QuestionService(ApplicationContext context, ILogger<QuestionService> logger, IMapper mapper)
        {
            _context = context;
            _logger = logger;
            _mapper = mapper;
        }

        public async Task<QuestionModelResponse> AddQuestion(QuestionModelRequest questionModel)
        {
            if (questionModel == null || string.IsNullOrWhiteSpace(questionModel.QuestionText) ||
                questionModel.CorrectAnswers.Count == 0)
            {
                throw new ArgumentException("Question Model was null");
            }

            questionModel.CorrectAnswers.ForEach(x =>
            { 
                if (string.IsNullOrWhiteSpace(x))
                {
                    throw new ArgumentException("Question Model was null");
                }
            });

            var correctAnswers = new List<CorrectAnswer>();
            questionModel.CorrectAnswers.ForEach(x => correctAnswers.Add(new CorrectAnswer(x)));
            var entity = _mapper.Map<Question>(questionModel);
            var result = await _context.Questions.AddAsync(entity);
            await _context.SaveChangesAsync();
            _logger.LogInformation($"Question id = {entity.Id} was added");
            return entity.ToQuestionModelResponse();
        }

        public QuestionModelResponse GetQuestion(long questionId)
        {
            DataValidation.ValidateId(questionId);

            var entity = _context.Questions.Where(x => x.Id == questionId)
                .Include(x => x.CorrectAnswers)
                .Include(x => x.Image).FirstOrDefault();
            DataValidation.ValidateEntity(entity, "Question");
            return entity.ToQuestionModelResponse();
        }

        public async Task UpdateQuestion(QuestionModelRequest questionModel, long questionId)
        {
            if (questionModel == null || string.IsNullOrWhiteSpace(questionModel.QuestionText) ||
                questionModel.CorrectAnswers.Count == 0)
            {
                throw new ArgumentException("Question Model was null");
            }

            DataValidation.ValidateId(questionId);

            var entity = _context.Questions.Where(x => x.Id == questionId).Include(x => x.CorrectAnswers).FirstOrDefault();
            DataValidation.ValidateEntity(entity, "Question");

            entity.QuestionText = questionModel.QuestionText ?? entity.QuestionText;
            if (questionModel.CorrectAnswers != null && questionModel.CorrectAnswers.Count != 0)
            {
                entity.CorrectAnswers = questionModel.CorrectAnswers.Select(x => new CorrectAnswer(x)).ToList();
            }

            await _context.SaveChangesAsync();
            _logger.LogInformation($"Question id = {entity.Id} was updated");
        }

        public async Task DeleteQuestion(long questionId)
        {
            DataValidation.ValidateId(questionId);

            var entity = _context.Set<Question>().FirstOrDefault(x => x.Id == questionId);
            DataValidation.ValidateEntity(entity, "Question");

            _context.Set<Question>().Remove(entity);
            await _context.SaveChangesAsync();
            _logger.LogInformation($"Question id = {entity.Id} was deleted");
        }

        public async Task<QuestionsCreatingModelResponse> CreateQuestions(QuestionsCreatingModelRequest questionsToAdd)
        {
            var questions = new QuestionsCreatingModelResponse();
            foreach (var question in questionsToAdd.Questions)
            {
                questions.Questions.Add(await AddQuestion(question));
            }

            await _context.SaveChangesAsync();
            return questions;
        }
    }
}
