using QuizWhois.Common.Models;

namespace QuizWhois.Domain.Services.Interfaces
{
    public interface IUserAnswerService
    {
        public QuestionModelResponse GetRandomQuestion();

        public bool CheckAnswer(UserAnswerModel operationModel);
    }
}
