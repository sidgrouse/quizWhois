using System;
using System.Linq;
using Microsoft.AspNetCore.Http;
using Microsoft.Extensions.Logging;
using QuizWhois.Common;
using QuizWhois.Common.Models;
using QuizWhois.Domain.Database;
using QuizWhois.Domain.Entity;
using QuizWhois.Domain.Services.Interfaces;

namespace QuizWhois.Domain.Services.Implementations
{
    public class QuestionRatingService : IQuestionRatingService
    {
        private readonly ApplicationContext _context;
        private readonly ILogger<QuestionRatingService> _logger;

        public QuestionRatingService(ApplicationContext context, ILogger<QuestionRatingService> logger)
        {
            _context = context;
            _logger = logger;
        }

        public double GetAverageRating(long questionId)
        {
            DataValidation.ValidateId(questionId);

            return _context.Set<QuestionRating>().Where(x => x.QuestionId == questionId)
                .Select(x => x.Value).ToList().ConvertAll(x => (double)x).Average();
        }

        public QuestionRatingModel AddRating(long questionId, long userId, uint rating)
        {
            DataValidation.ValidateId(questionId);
            DataValidation.ValidateId(userId);

            if (rating > 5 || rating < 1)
            {
                throw new Exception("Rating less then 1 or more than 5");
            }

            if (_context.Set<QuestionRating>().FirstOrDefault(x => x.QuestionId == questionId && x.UserId == userId) != null)
            {
                throw new Exception("This rating already exist");
            }

            var entity = new QuestionRating(questionId, userId, rating);
            _context.Set<QuestionRating>().Add(entity);
            _context.SaveChanges();
            _logger.LogInformation($"QuestionRating id = {entity.Id}, questionId = {entity.QuestionId}, userId = {entity.UserId} was added");
            return new QuestionRatingModel(entity.Id, entity.QuestionId, entity.UserId, entity.Value);
        }

        public QuestionRatingModel UpdateRating(long questionId, long userId, uint rating)
        {
            DataValidation.ValidateId(questionId);
            DataValidation.ValidateId(userId);

            if (rating > 5 || rating < 1)
            {
                throw new Exception("Rating more than 5");
            }

            var entity = _context.Set<QuestionRating>().FirstOrDefault(x => x.QuestionId == questionId && x.UserId == userId);
            if (entity == null)
            {
                throw new Exception("Rating is not found");
            }

            entity.Value = rating;
            _context.Set<QuestionRating>().Update(entity);
            _context.SaveChanges();
            _logger.LogInformation($"QuestionRating id = {entity.Id}, questionId = {entity.QuestionId}, userId = {entity.UserId} was updated");
            return new QuestionRatingModel(entity.Id, entity.QuestionId, entity.UserId, entity.Value);
        }

        public bool DeleteRating(long questionId, long userId)
        {
            DataValidation.ValidateId(questionId);
            DataValidation.ValidateId(userId);

            var entity = _context.Set<QuestionRating>().FirstOrDefault(x => x.QuestionId == questionId && x.UserId == userId);
            if (entity == null)
            {
                throw new Exception("Rating is not found");
            }

            _context.Set<QuestionRating>().Remove(entity);
            _context.SaveChanges();
            _logger.LogInformation($"QuestionRating id = {entity.Id}, questionId = {entity.QuestionId}, userId = {entity.UserId} was deleted");
            return true;
        }
    }
}
