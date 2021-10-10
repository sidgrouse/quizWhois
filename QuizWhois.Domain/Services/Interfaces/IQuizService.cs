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
        public Task<QuizModel> AddToQuiz(long quiz, long question); //, int index

        public Task<QuizModel> FormQuiz(List<long> question, string quizName = "");
    }
}
