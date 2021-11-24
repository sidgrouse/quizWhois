using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Http;
using QuizWhois.Domain.Services.Interfaces;

namespace QuizWhois.Domain.Services.Implementations
{
    public class QuestionImageService : IQuestionImageService
    {
        public bool AddOrReplaceImage(long questionId, IFormFile image)
        {
            throw new NotImplementedException();
        }

        public bool DeleteImage(long questionId)
        {
            throw new NotImplementedException();
        }
    }
}
