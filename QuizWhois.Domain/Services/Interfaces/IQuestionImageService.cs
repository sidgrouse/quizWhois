using System.Threading.Tasks;
using Microsoft.AspNetCore.Http;
using QuizWhois.Common.Models;

namespace QuizWhois.Domain.Services.Interfaces
{
    public interface IQuestionImageService
    {
        public Task<QuestionImageResponse> GetQuestionImage(long questionId);

        public Task<bool> AddOrReplaceImage(IFormFile image, QuestionImageRequest imageInfo);

        public Task DeleteImage(long questionId);
    }
}
