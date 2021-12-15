namespace QuizWhois.Domain.Entity
{
    public class CorrectAnswer
    {
        public long Id { get; set; }

        public long QuestionId { get; set; }

        public Question Question { get; set; }

        public string AnswerText { get; set; }

        public CorrectAnswer(string answerText)
        {
            AnswerText = answerText;
        }
    }
}
