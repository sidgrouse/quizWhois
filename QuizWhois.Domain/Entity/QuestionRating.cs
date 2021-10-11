namespace QuizWhois.Domain.Entity
{
    public class QuestionRating
    {
        public long Id { get; set; }
        public uint Value { get; set; }
        public long UserId { get; set; }
        public User User { get; set; }
        public long QuestionId { get; set; }
        public Question Question { get; set; }

        public QuestionRating(long questionId, long userId, uint value)
        {
            QuestionId = questionId;
            UserId = userId;
            Value = value;
        }
    }
}
