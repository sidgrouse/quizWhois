using QuizWhois.Common.Models.Base;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace QuizWhois.Common.Models
{
    public class QuestionModel : BaseModel
    {
        public string QuestionText { get; set; }
        public string CorrectAnswer { get; set; }
    }
}
