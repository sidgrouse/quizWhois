﻿using System.Collections.Generic;

namespace QuizWhois.Domain.Entity
{
    public class Question
    {
        public long Id { get; set; }

        public string QuestionText { get; set; }

        public List<CorrectAnswer> CorrectAnswers { get; set; }

        public long QuizId { get; set; }

        public QuestionImage QuestionImage { get; set; }
    }
}
