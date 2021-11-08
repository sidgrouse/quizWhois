using System.Collections.Generic;

namespace QuizWhois.Domain.Entity
{
    public class Question
    {
        public long Id { get; set; }

        public string QuestionText { get; set; }

        public List<CorrectAnswer> CorrectAnswers { get; set; } = new List<CorrectAnswer>();

        public Question(string questionText, List<string> correctAnswers)
        {
            QuestionText = questionText;
            correctAnswers.ForEach(x => CorrectAnswers.Add(new CorrectAnswer(x)));
        }
    }
}
