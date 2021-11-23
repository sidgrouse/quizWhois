namespace QuizWhois.Common.Models
{
    public class HintModel
    {
        public long Id { get; }

        public long QuestionId { get; set; }

        public string Text { get; set; }

        public HintModel(long id, long questionId, string text)
        {
            Id = id;
            QuestionId = questionId;
            Text = text;
        }
    }
}
