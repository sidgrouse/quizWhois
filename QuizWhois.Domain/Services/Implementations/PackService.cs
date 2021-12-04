using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Logging;
using QuizWhois.Common;
using QuizWhois.Common.Models;
using QuizWhois.Domain.Database;
using QuizWhois.Domain.Entity;
using QuizWhois.Domain.Services.Interfaces;

namespace QuizWhois.Domain.Services.Implementations
{
    public class PackService : IPackService
    {
        private readonly ApplicationContext _dbContext;
        private readonly ILogger<PackService> _logger;

        public PackService(ApplicationContext context, ILogger<PackService> logger)
        {
            _dbContext = context;
            _logger = logger;
        }

        public async Task<PackModelResponse> CreatePack(PackModelRequest packModel)
        {
            if (packModel == null || !packModel.IsDraft.HasValue)
            {
                throw new ArgumentException("Pack Model or IsDraft field was null");
            }

            var packToSave = new Pack(packModel.Name, packModel.Description, packModel.IsDraft.Value);
            await _dbContext.AddAsync(packToSave);
            await _dbContext.SaveChangesAsync();
            _logger.LogInformation($"Pack id = {packToSave.Id} was saved");
            return new PackModelResponse(packToSave.Id, packToSave.Name, packToSave.Description, packToSave.IsDraft);
        }

        public PackModelResponse GetPack(long packId)
        {
            DataValidation.ValidateId(packId);

            var packFromDb = _dbContext.Packs.Where(x => x.Id == packId).Include(y => y.Questions.Where(z => z.PackId == packId))
                .ThenInclude(a => a.CorrectAnswers.Where(b => b.Question.PackId == packId)).FirstOrDefault();
            var result = new PackModelResponse(packFromDb.Id, packFromDb.Name, packFromDb.Description, packFromDb.IsDraft);
            result.Questions = packFromDb.Questions
                .Select(x => new QuestionModelResponse(x.Id, x.QuestionText, x.CorrectAnswers.Select(y => y.AnswerText).ToList(), x.PackId));

            return result;
        }

        public DraftPacksModelResponse GetDraftPacks()
        {
            var packsFromDb = _dbContext.Packs.Where(x => x.IsDraft == true).Include(y => y.Questions.Where(z => z.Pack.IsDraft == true))
                .ThenInclude(a => a.CorrectAnswers.Where(b => b.Question.Pack.IsDraft == true)).ToList();
            var result = new DraftPacksModelResponse(new List<PackModelResponse>());
            packsFromDb.ForEach(x => result.DraftPacks.Add(new PackModelResponse(x.Id, x.Name, x.Description, x.IsDraft)));
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
