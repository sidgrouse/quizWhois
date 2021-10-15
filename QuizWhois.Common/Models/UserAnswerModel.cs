using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

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
