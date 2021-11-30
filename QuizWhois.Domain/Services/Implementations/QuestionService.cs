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
            if (questionModel == null || questionModel.QuestionText == string.Empty || questionModel.CorrectAnswers.Count == 0)
            {
                throw new Exception("Operation Model was null");
            }

            var correctAnswers = new List<CorrectAnswer>();
            questionModel.CorrectAnswers.ForEach(x => correctAnswers.Add(new CorrectAnswer(x)));
            var entity = _mapper.Map<Question>(questionModel);
            var result = await _context.Questions.AddAsync(entity);
            await _context.SaveChangesAsync();
            _logger.LogInformation($"Question id = {entity.Id} was added");
            var response = _mapper.Map<QuestionModelResponse>(result.Entity);
            return response;
            /*var correctAnswersToModel = new List<string>();
            result.Entity.CorrectAnswers.ForEach(x => correctAnswersToModel.Add(x.AnswerText));
            return new QuestionModelResponse(result.Entity.Id, result.Entity.QuestionText, correctAnswersToModel, result.Entity.PackId);*/
        }

        public QuestionModelResponse GetQuestion(long questionId)
        {
            DataValidation.ValidateId(questionId);
            var entity = _context.Questions.Where(x => x.Id == questionId)
                .Include(x => x.CorrectAnswers).FirstOrDefault();
            return _mapper.Map<QuestionModelResponse>(entity);
            /*var correctAnswers = new List<string>();
            entity.correctAnswers.ToList().ForEach(x => correctAnswers.Add(x.AnswerText));
            return new QuestionModelResponse(entity.id, entity.questionText, correctAnswers, entity.packId);*/
        }

        public async Task UpdateQuestion(QuestionModelRequest questionModel, long questionId)
        {
            DataValidation.ValidateId(questionId);

            var entity = _context.Questions.Where(x => x.Id == questionId).Include(x => x.CorrectAnswers).FirstOrDefault();
            if (entity == null)
            {
                throw new Exception("Question is not found");
            }

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
            if (entity == null)
            {
                throw new Exception("Question is not found");
            }

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
