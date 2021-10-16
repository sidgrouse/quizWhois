namespace QuizWhois.Common.Models
{
    public class QuestionModel
    {
        public long Id { get; set; }

        public string QuestionText { get; set; }

        public string CorrectAnswer { get; set; }

        public QuestionModel(long id, string questionText, string correctAnswer)
        {
            Id = id;
            QuestionText = questionText;
            CorrectAnswer = correctAnswer;
        }
    }
}
