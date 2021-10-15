namespace QuizWhois.Domain.Entity
{
    public class Question
    {
        public long Id { get; set; }

        public string QuestionText { get; set; }

        public string CorrectAnswer { get; set; }

        public Question(string questionText, string correctAnswer)
        {
            QuestionText = questionText;
            CorrectAnswer = correctAnswer;
        }
    }
}
