using System;
using System.IO;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Http;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Logging;
using QuizWhois.Common;
using QuizWhois.Common.Models;
using QuizWhois.Domain.Database;
using QuizWhois.Domain.Entity;
using QuizWhois.Domain.Services.Interfaces;

namespace QuizWhois.Domain.Services.Implementations
{
    public class QuestionImageService : IQuestionImageService
    {
        private readonly ApplicationContext _context;
        private readonly ILogger<QuestionImageService> _logger;

        public QuestionImageService(ApplicationContext context, ILogger<QuestionImageService> logger)
        {
            _context = context;
            _logger = logger;
        }

        public async Task<bool> AddOrReplaceImage(IFormFile image, string caption, long questionId)
        {
            DataValidation.ValidateId(questionId);

            if (image.ContentType == "image/jpeg" && image.Length > 0)
            {
                var question = GetQuestionById(questionId);

                if (question != null)
                {
                    using var ms = new MemoryStream();
                    await image.CopyToAsync(ms);
                    var imageBytes = ms.ToArray();

                    question.Image = new () 
                    {
                       QuestionId = questionId,
                       Caption = caption,
                       Name = image.FileName,
                       ImageData = imageBytes                       
                    };
                    await _context.SaveChangesAsync();
                    return true;
                }
                else
                {
                    throw new ArgumentNullException("Image file is empty!");
                }
            }
            else
            {
                throw new ArgumentException("Wrong image format!");
            }
        }

        public async Task DeleteImage(long questionId)
        {
            DataValidation.ValidateId(questionId);
            var question = GetQuestionById(questionId);
            question.Image = null;
            await _context.SaveChangesAsync();
        }

        public async Task<QuestionImageResponse> GetQuestionImage(long questionId)
        {
            DataValidation.ValidateId(questionId);

            var image = await _context.Set<QuestionImage>().FirstOrDefaultAsync(x => x.QuestionId == questionId);

            return new QuestionImageResponse()
            {
                Caption = image.Caption,
                ImageData = image.ImageData,
                Name = image.Name,
                QuestionId = questionId
            };
        }

        private Entity.Question GetQuestionById(long questionId)
        {
            return _context.Questions.Where(x => x.Id == questionId).Include(x => x.Image).FirstOrDefault();
        }
    }
}
