using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using QuizWhois.Common.Models;

namespace QuizWhois.Domain.Services.Interfaces
{
    public interface IQuizService
    {
        public Task AddToQuiz(AddToSetModel[] addToSet);

        public Task<QuizModel> FormQuiz(List<long> question, string quizName = "");
    }
}
