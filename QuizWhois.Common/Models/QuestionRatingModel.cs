namespace QuizWhois.Common.Models
{
    public class QuestionRatingModel
    {
        public long Id { get; set; }
        public long QuestionId { get; set; }
        public long UserId { get; set; }
        public uint Value { get; set; }

        public QuestionRatingModel(long id, long questionId, long userId, uint value)
        {
            Id = id;
            QuestionId = questionId;
            UserId = userId;
            Value = value;
        }
    }
}
