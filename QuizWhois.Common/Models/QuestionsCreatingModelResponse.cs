using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

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
