using System.Collections.Generic;

namespace QuizWhois.Common.Models
{
    public class QuestionModel
    {
        public long Id { get; set; }

        public string QuestionText { get; set; }

        public List<string> CorrectAnswers { get; set; }

        public long PackId { get; set; }

        public QuestionModel(long id, string questionText, List<string> correctAnswers, long packId)
        {
            Id = id;
            QuestionText = questionText;
            CorrectAnswers = correctAnswers;
            PackId = packId;
        }
    }
}
