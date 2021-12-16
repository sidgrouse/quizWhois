using System.Collections.Generic;

namespace QuizWhois.Domain.Entity
{
    public class Question
    {
        public long Id { get; set; }

        public string QuestionText { get; set; }

        public List<CorrectAnswer> CorrectAnswers { get; set; }

        public Pack Pack { get; set; }
        
        public long PackId { get; set; }

        public List<Hint> Hints { get; set; }

        public QuestionImage Image { get; set; }
    }
}
