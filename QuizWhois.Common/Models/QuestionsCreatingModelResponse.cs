using System.Collections.Generic;

namespace QuizWhois.Common.Models
{
    public class QuestionsCreatingModelResponse
    {
        public List<QuestionModelResponse> Questions { get; set; }

        public QuestionsCreatingModelResponse()
        {
            Questions = new List<QuestionModelResponse>();
        }
    }
}
