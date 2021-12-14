namespace QuizWhois.Common.Models
{
    public class UserAnswerModel
    {
        public long Id { get; set; }

        public string UserAnswerText { get; set; }

        public UserAnswerModel(long id, string userAnswerText)
        {
            Id = id;
            UserAnswerText = userAnswerText;
        }
    }
}
