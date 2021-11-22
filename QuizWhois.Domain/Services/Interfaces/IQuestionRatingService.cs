using Microsoft.AspNetCore.Http;
using QuizWhois.Common.Models;

namespace QuizWhois.Domain.Services.Interfaces
{
    public interface IQuestionRatingService
    {
        public double GetAverageRating(long questionid);

        public QuestionRatingModel AddRating(long questionModelId, long userId, uint rating);

        public QuestionRatingModel UpdateRating(long questionModelId, long userId, uint rating);

        public bool DeleteRating(long questionModelId, long userId);

        public bool AddOrReplaceImage(long questionId, IFormFile image);

        public bool DeleteImage(long questionId);
    }
}
