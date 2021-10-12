using QuizWhois.Common.Models;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace QuizWhois.Domain.Services.Interfaces
{
    public interface IQuestionService
    {
        public QuestionModel AddQuestion(QuestionModel operationModel);

        public Task AddMany(IEnumerable<QuestionModel> questionsToAdd);
    }
}
