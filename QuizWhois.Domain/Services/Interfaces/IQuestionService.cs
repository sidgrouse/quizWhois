using System.Collections.Generic;
using System.Threading.Tasks;
using QuizWhois.Common.Models;

namespace QuizWhois.Domain.Services.Interfaces
{
    public interface IQuestionService
    {
        public Task<QuestionModel> AddQuestion(QuestionModel operationModel);

        public Task AddMany(IEnumerable<QuestionModel> questionsToAdd);
    }
}
