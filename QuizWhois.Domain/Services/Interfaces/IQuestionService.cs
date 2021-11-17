using System.Collections.Generic;
using System.Threading.Tasks;
using QuizWhois.Common.Models;

namespace QuizWhois.Domain.Services.Interfaces
{
    public interface IQuestionService
    {
        public Task<QuestionModel> AddQuestion(QuestionModel operationModel);

        public QuestionModel GetQuestion(long questionId);

        public Task CreateQuestions(IEnumerable<QuestionModel> questionsToAdd);

        public Task UpdateQuestion(QuestionModel questionModel, long questionId);

        public Task DeleteQuestion(long questionId);
    }
}
