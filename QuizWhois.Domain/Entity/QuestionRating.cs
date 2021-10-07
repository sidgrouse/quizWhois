using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace QuizWhois.Domain.Entity
{
    public class QuestionRating
    {
        public long Id { get; set; }
        public uint RatingNumber { get; set; }
        public long UserId { get; set; }
        public User User { get; set; }
        public long QuestionId { get; set; }
        public Question Question { get; set; }

        public QuestionRating(long questionId, long userId, uint ratingNumber)
        {
            QuestionId = questionId;
            UserId = userId;
            RatingNumber = ratingNumber;
        }
    }
}
