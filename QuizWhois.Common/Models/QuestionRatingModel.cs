using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace QuizWhois.Common.Models
{
    public class QuestionRatingModel
    {
        public long Id { get; set; }
        public long QuestionId { get; set; }
        public long UserId { get; set; }
        public uint RatingNumber { get; set; }

        public QuestionRatingModel(long id, long questionId, long userId, uint ratingNumber)
        {
            Id = id;
            QuestionId = questionId;
            UserId = userId;
            RatingNumber = ratingNumber;
        }
    }
}
