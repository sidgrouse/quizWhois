using System.Collections.Generic;

namespace QuizWhois.Common.Models
{
    public class AddToSetModel
    {
        public long QuizId { get; set; }

        public IEnumerable<long> QuestionIds { get; set; }
    }
}