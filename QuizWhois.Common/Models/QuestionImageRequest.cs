namespace QuizWhois.Common.Models
{
    public class QuestionImageRequest
    {
        public string Caption { get; set; }

        public int QuestionId { get; set; }

        public QuestionImageRequest(int questionId, string caption)
        {
            QuestionId = questionId;
            Caption = caption;
        }
    }
}