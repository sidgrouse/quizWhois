using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace QuizWhois.Domain.Entity
{
    public class Question
    {
        public long Id { get; set; }
        public string QuestionText { get; set; }
        public string CorrectAnswer { get; set; }

        public Question(string questionText, string correctAnswer)
        {
            QuestionText = questionText;
            CorrectAnswer = correctAnswer;
        }
    }
}
