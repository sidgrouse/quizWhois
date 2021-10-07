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
            return _context.Set<QuestionRating>().Where(x => x.QuestionId == questionId).Select(x => x.RatingNumber)
                .ToList().ConvertAll(x => (double)x).Average();
        }

        public QuestionRatingModel AddRating(QuestionModel questionModel, long userId, uint rating)
        {
            if (questionModel == null || userId <= 0)
            {
                throw new Exception("Invalid data");
            }
            if (rating > 5)
            {
                throw new Exception("Rating more than 5");
            }
            var entity = new QuestionRating(questionModel.Id, userId, rating);
            _context.Set<QuestionRating>().Add(entity);
            _context.SaveChanges();
            return new QuestionRatingModel(entity.Id, entity.QuestionId, entity.UserId, entity.RatingNumber);
        }

        public QuestionRatingModel UpdateRating(QuestionModel questionModel, long userId, uint rating)
        {
            var entity = _context.Set<QuestionRating>().FirstOrDefault(x => x.QuestionId == questionModel.Id && x.UserId == userId);
            entity.RatingNumber = rating;
            _context.Set<QuestionRating>().Update(entity);
            return new QuestionRatingModel(entity.Id, entity.QuestionId, entity.UserId, entity.RatingNumber);
        }
    }
}
