using QuizWhois.Common.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace QuizWhois.Domain.Services.Interfaces
{
    public interface IQuestionRatingService
    {
        public double GetAverageRating(long questionid);
        public QuestionRatingModel AddRating(QuestionModel questionModel, long userId, uint rating);
        public QuestionRatingModel UpdateRating(QuestionModel questionModel, long userId, uint rating);
        public bool DeleteRating(QuestionModel questionModel, long userId);
    }
}
