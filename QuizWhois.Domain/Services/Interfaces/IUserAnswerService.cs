﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using QuizWhois.Common.Models;

namespace QuizWhois.Domain.Services.Interfaces
{
    public interface IUserAnswerService
    {
        public QuestionModel GetRandomQuestion();
        public bool CheckAnswer(UserAnswerModel operationModel);
    }
}
