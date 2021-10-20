namespace QuizWhois.Domain.Entity
{
    public class Hint
    {
        public long Id { get; set; }
        public string Text { get; set; }
        public long QuestionId { get; set; }
        public Question Question { get; set; }
        public Hint(long questionId, string text)
        {
            QuestionId = questionId;
            Text = text;
        }
    }
}
