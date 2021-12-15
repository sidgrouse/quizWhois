using System.Collections.Generic;

namespace QuizWhois.Common.Models
{
    public class QuestionModelResponse
    {
        public long Id { get; set; }

        public string QuestionText { get; set; }

        public List<string> CorrectAnswers { get; set; }

        public long PackId { get; set; }

        public bool HasImage { get; set; }

        public QuestionModelResponse(long id, string questionText, List<string> correctAnswers, long packId, bool hasImage)
        {
            Id = id;
            QuestionText = questionText;
            CorrectAnswers = correctAnswers;
            PackId = packId;
            HasImage = hasImage;
        }
    }
}
