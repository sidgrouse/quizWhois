using System.Threading.Tasks;
using Microsoft.AspNetCore.Http;
using QuizWhois.Common.Models;

namespace QuizWhois.Domain.Services.Interfaces
{
    public interface IQuestionImageService
    {
        public Task<QuestionImageResponse> GetQuestionImage(long questionId);

        public Task<bool> AddOrReplaceImage(IFormFile image, string caption, long questionId);

        public Task<bool> DeleteImage(long questionId);
    }
}
