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
                var question = await GetQuestionById(questionId);

                if (question != null)
                {
                    var imageBytes = new byte[image.Length];
                    var ms = new MemoryStream();                    
                    await image.CopyToAsync(ms);
                    imageBytes = ms.ToArray();
                    ms.Dispose();

                    question.Image = new () 
                    {
                       QuestionId = questionId,
                       Caption = caption,
                       Name = image.FileName,
                       ImageData = imageBytes                       
                    };
                    
                    var result = await _context.Set<QuestionImage>().AddAsync(question.Image);
                    await _context.SaveChangesAsync();                    

                    return result != null;
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
            var question = await GetQuestionById(questionId);
            question.Image = null;
            await _context.SaveChangesAsync();
        }

        public async Task<QuestionImageResponse> GetQuestionImage(long questionId)
        {
            DataValidation.ValidateId(questionId);

            var image = await _context.Set<QuestionImage>().FirstOrDefaultAsync(x => x.QuestionId == questionId);

            if (image == null)
            {
                throw new ArgumentException("Question does not have an image");
            }

            return new QuestionImageResponse()
            {
                Caption = image.Caption,
                ImageData = image.ImageData,
                Name = image.Name,
                QuestionId = questionId
            };
        }

        private async Task<Entity.Question> GetQuestionById(long questionId)
        {
            return await _context.Questions.Where(x => x.Id == questionId).Include(x => x.Image).FirstOrDefaultAsync();
        }
    }
}
