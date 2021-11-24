using Microsoft.AspNetCore.Http;

namespace QuizWhois.Domain.Services.Interfaces
{
    public interface IQuestionImageService
    {
        public bool AddOrReplaceImage(long questionId, IFormFile image);

        public bool DeleteImage(long questionId);
    }
}
