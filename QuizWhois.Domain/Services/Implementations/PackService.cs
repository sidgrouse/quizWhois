using System;
using System.Linq;
using System.Threading.Tasks;
using AutoMapper;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Logging;
using QuizWhois.Common;
using QuizWhois.Common.Models;
using QuizWhois.Domain.Database;
using QuizWhois.Domain.Entity;
using QuizWhois.Domain.Services.Interfaces;
using QuizWhois.Domain.Services.Mapper;

namespace QuizWhois.Domain.Services.Implementations
{
    public class PackService : IPackService
    {
        private readonly ApplicationContext _dbContext;
        private readonly ILogger<PackService> _logger;
        private readonly IMapper _mapper;

        public PackService(ApplicationContext context, ILogger<PackService> logger, IMapper mapper)
        {
            _dbContext = context;
            _logger = logger;
            _mapper = mapper;
        }

        public async Task<PackModelResponse> CreatePack(PackModelRequest packModel)
        {
            if (packModel == null || !packModel.IsDraft.HasValue)
            {
                throw new ArgumentException("Pack Model or IsDraft field was null");
            }

            var packToSave = _mapper.Map<Pack>(packModel);
            var result = await _dbContext.AddAsync(packToSave);
            await _dbContext.SaveChangesAsync();
            _logger.LogInformation($"Pack id = {packToSave.Id} was saved");
            return result.Entity.ToPackModelResponse();
        }

        public PackModelResponse GetPack(long packId)
        {
            DataValidation.ValidateId(packId);

            var entity = _dbContext.Packs.Where(x => x.Id == packId)
                .Include(x => x.Questions)
                .ThenInclude(x => x.CorrectAnswers)
                .Include(x => x.Questions).ThenInclude(x => x.Image).FirstOrDefault();
            return entity.ToPackModelResponse();
        }

        public DraftPacksModelResponse GetDraftPacks()
        {
            var packsFromDb = _dbContext.Packs.Where(x => x.IsDraft == true).Include(y => y.Questions.Where(z => z.Pack.IsDraft == true))
                .ThenInclude(a => a.CorrectAnswers.Where(b => b.Question.Pack.IsDraft == true)).ToList();
            var result = new DraftPacksModelResponse(new List<PackModelResponse>());
            packsFromDb.ForEach(x => 
                {
                    var packResult = new PackModelResponse(x.Id, x.Name, x.Description, x.IsDraft);
                    var questionResult = new List<QuestionModelResponse>();
                    x.Questions.ForEach(y =>
                    {
                        var correctAnswerResult = new List<string>();
                        y.CorrectAnswers.ForEach(z => correctAnswerResult.Add(z.AnswerText));
                        questionResult.Add(new QuestionModelResponse(y.Id, y.QuestionText, correctAnswerResult, y.PackId));
                    });
                    packResult.Questions = new List<QuestionModelResponse>(questionResult);
                    result.DraftPacks.Add(packResult);
                });
            return result;
        }

        public async Task UpdatePack(PackModelRequest packModel, long packId)
        {
            DataValidation.ValidateId(packId);

            var entity = _dbContext.Set<Pack>().FirstOrDefault(x => x.Id == packId);
            if (entity == null)
            {
                throw new ArgumentException("Pack is not found");
            }

            if (!string.IsNullOrWhiteSpace(packModel.Name))
            {
                entity.Name = packModel.Name;
            }

            if (!string.IsNullOrWhiteSpace(packModel.Description))
            {
                entity.Description = packModel.Description;
            }

            if (packModel.IsDraft != null)
            {
                entity.IsDraft = packModel.IsDraft.Value;
            }

            await _dbContext.SaveChangesAsync();
            _logger.LogInformation($"Pack id = {entity.Id} was updated");
        }

        public async Task DeletePack(long packId)
        {
            DataValidation.ValidateId(packId);

            var entity = _dbContext.Set<Pack>().FirstOrDefault(x => x.Id == packId);
            if (entity == null)
            {
                throw new Exception("Pack is not found");
            }

            _dbContext.Set<Pack>().Remove(entity);
            await _dbContext.SaveChangesAsync();
            _logger.LogInformation($"Pack id = {entity.Id} was deleted");
        }
    }
}
