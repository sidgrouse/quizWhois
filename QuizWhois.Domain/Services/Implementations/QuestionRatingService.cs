using QuizWhois.Common.Models;
using QuizWhois.Domain.Database;
using QuizWhois.Domain.Entity;
using QuizWhois.Domain.Services.Interfaces;
using System;
using System.Linq;

namespace QuizWhois.Domain.Services.Implementations
{
    public class QuestionRatingService : IQuestionRatingService
    {
        public ApplicationContext _context { get; set; }

        public QuestionRatingService(ApplicationContext context)
        {
            _context = context;
        }

        public double GetAverageRating(long questionId)
        {
            if (questionId <= 0)
            {
                throw new Exception("Invalid data");
            }
            return _context.Set<QuestionRating>().Where(x => x.QuestionId == questionId)
                .Select(x => x.Value).ToList().ConvertAll(x => (double)x).Average();
        }

        public QuestionRatingModel AddRating(long questionModelId, long userId, uint rating)
        {
            if (questionModelId <= 0 || userId <= 0)
            {
                throw new Exception("Invalid data");
            }
            if (rating > 5 || rating < 1)
            {
                throw new Exception("Rating more than 5");
            }
            if (_context.Set<QuestionRating>().FirstOrDefault(x => x.QuestionId == questionModelId && x.UserId == userId) != null)
            {
                throw new Exception("This rating already exist");
            }
            var entity = new QuestionRating(questionModelId, userId, rating);
            _context.Set<QuestionRating>().Add(entity);
            _context.SaveChanges();
            return new QuestionRatingModel(entity.Id, entity.QuestionId, entity.UserId, entity.Value);
        }

        public QuestionRatingModel UpdateRating(long questionModelId, long userId, uint rating)
        {
            if (questionModelId <= 0 || userId <= 0)
            {
                throw new Exception("Invalid data");
            }
            if (rating > 5 || rating < 1)
            {
                throw new Exception("Rating more than 5");
            }
            var entity = _context.Set<QuestionRating>().FirstOrDefault(x => x.QuestionId == questionModelId && x.UserId == userId);
            if (entity == null)
            {
                throw new Exception("Rating is not found");
            }
            entity.Value = rating;
            _context.Set<QuestionRating>().Update(entity);
            _context.SaveChanges();
            return new QuestionRatingModel(entity.Id, entity.QuestionId, entity.UserId, entity.Value);
        }

        public bool DeleteRating(long questionModelId, long userId)
        {
            if (questionModelId <= 0 || userId <= 0)
            {
                throw new Exception("Invalid data");
            }
            var entity = _context.Set<QuestionRating>().FirstOrDefault(x => x.QuestionId == questionModelId && x.UserId == userId);
            if (entity == null)
            {
                throw new Exception("Rating is not found");
            }
            _context.Set<QuestionRating>().Remove(entity);
            _context.SaveChanges();
            return true;
        }
    }
}
