using QuizWhois.Common.Models;

namespace QuizWhois.Domain.Services.Interfaces
{
    public interface IHintService
    {
        public HintModel AddHint(long questionId, string text);
    }
}
