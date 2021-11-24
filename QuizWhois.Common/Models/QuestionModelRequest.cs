using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace QuizWhois.Common.Models
{
    public class QuestionModelRequest
    {
        public string QuestionText { get; set; }

        public List<string> CorrectAnswers { get; set; }

        public long PackId { get; set; }

        public QuestionModelRequest(string questionText, List<string> correctAnswers, long packId)
        {
            QuestionText = questionText;
            CorrectAnswers = correctAnswers;
            PackId = packId;
        }
    }
}
