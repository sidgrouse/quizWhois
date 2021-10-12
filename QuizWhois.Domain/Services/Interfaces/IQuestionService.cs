using QuizWhois.Common.Models;

namespace QuizWhois.Domain.Services.Interfaces
{
    public interface IQuestionService
    {
        public QuestionModel AddQuestion(QuestionModel operationModel);

        public Task AddMany(IEnumerable<QuestionModel> questionsToAdd);
    }
}
