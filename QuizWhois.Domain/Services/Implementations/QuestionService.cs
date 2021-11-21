using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
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

        public QuestionService(ApplicationContext context, ILogger<QuestionService> logger)
        {
            _context = context;
            _logger = logger;
        }

        public async Task<QuestionModelResponse> AddQuestion(QuestionModelRequest questionModel)
        {
            if (questionModel == null || questionModel.QuestionText == string.Empty || questionModel.CorrectAnswers.Count == 0)
            {
                throw new Exception("Operation Model was null");
            }

            var correctAnswers = new List<CorrectAnswer>();
            questionModel.CorrectAnswers.ForEach(x => correctAnswers.Add(new CorrectAnswer(x)));
            var entity = new Question { QuestionText = questionModel.QuestionText, CorrectAnswers = correctAnswers, PackId = questionModel.PackId };
            var result = await _context.Questions.AddAsync(entity);
            await _context.SaveChangesAsync();
            _logger.LogInformation($"Question id = {entity.Id} was added");
            var correctAnswersToModel = new List<string>();
            result.Entity.CorrectAnswers.ForEach(x => correctAnswersToModel.Add(x.AnswerText));
            return new QuestionModelResponse(result.Entity.Id, result.Entity.QuestionText, correctAnswersToModel, result.Entity.PackId);
        }

        public QuestionModelResponse GetQuestion(long questionId)
        {
            DataValidation.ValidateId(questionId);

            var entity = _context.Questions.Where(x => x.Id == questionId).Select(x => new
                {
                    id = x.Id,
                    questionText = x.QuestionText,
                    correctAnswers = x.CorrectAnswers,
                    packId = x.PackId
                }).FirstOrDefault();

            var correctAnswers = new List<string>();
            entity.correctAnswers.ToList().ForEach(x => correctAnswers.Add(x.AnswerText));
            return new QuestionModelResponse(entity.id, entity.questionText, correctAnswers, entity.packId);
        }

        public async Task UpdateQuestion(QuestionModelRequest questionModel, long questionId)
        {
            DataValidation.ValidateId(questionId);

            var entity = _context.Questions.Where(x => x.Id == questionId).Select(x => new Question
            {
                Id = x.Id,
                QuestionText = x.QuestionText,
                CorrectAnswers = x.CorrectAnswers,
                PackId = x.PackId
            }).FirstOrDefault();
            if (entity == null)
            {
                throw new Exception("Question is not found");
            }

            if (questionModel.QuestionText != string.Empty)
            {
                entity.QuestionText = questionModel.QuestionText;
            }

            if (questionModel.CorrectAnswers != null && questionModel.CorrectAnswers.Count != 0)
            {
                _context.Set<CorrectAnswer>().RemoveRange(entity.CorrectAnswers);
                entity.CorrectAnswers.Clear();
                questionModel.CorrectAnswers.ForEach(x => entity.CorrectAnswers.Add(new CorrectAnswer(x)));
                _context.Set<CorrectAnswer>().AddRange(entity.CorrectAnswers);
            }

            _context.Set<Question>().Update(entity);
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

        public async Task<List<QuestionModelResponse>> CreateQuestions(IEnumerable<QuestionModelRequest> questionsToAdd)
        {
            var questions = new List<QuestionModelResponse>();
            foreach (var question in questionsToAdd)
            {
               questions.Add(await AddQuestion(question));
            }

            await _context.SaveChangesAsync();
            return questions;
        }
    }
}
