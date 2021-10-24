namespace QuizWhois.Api.Models
{
    public class QuestionRatingRequest
    {
        public long UserId { get; set; }

        public uint Mark { get; set; }
    }
}
