using System.Threading.Tasks;
using QuizWhois.Common.Models;

namespace QuizWhois.Domain.Services.Interfaces
{
    public interface IQuestionService
    {
        public Task<QuestionModelResponse> AddQuestion(QuestionModelRequest operationModel);

        public QuestionModelResponse GetQuestion(long questionId);

        public Task<QuestionsCreatingModelResponse> CreateQuestions(QuestionsCreatingModelRequest questionsToAdd);

        public Task UpdateQuestion(QuestionModelRequest questionModel, long questionId);

        public Task DeleteQuestion(long questionId);
    }
}
