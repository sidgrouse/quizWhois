using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using AutoMapper;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Query;
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
            if (packModel == null || string.IsNullOrWhiteSpace(packModel.Name) ||
                string.IsNullOrWhiteSpace(packModel.Description) || !packModel.IsDraft.HasValue)
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
            DataValidation.ValidateEntity(entity, "Pack");

            return entity.ToPackModelResponse();
        }

        public PacksModelResponse GetPacks(bool? isDraft = null)
        {
            var entities = _dbContext.Packs
                .Include(x => x.Questions)
                .ThenInclude(x => x.CorrectAnswers)
                .Include(x => x.Questions).ThenInclude(x => x.Image);
            IQueryable<Pack> packs;
            if (isDraft != null)
            {
                packs = entities.Where(x => x.IsDraft == isDraft);
            }
            else
            {
                packs = entities;
            }

            var result = new PacksModelResponse(new List<PackModelResponse>());
            packs.ToList().ForEach(x => result.Packs.Add(x.ToPackModelResponse()));
            return result;
        }

        public async Task UpdatePack(PackModelRequest packModel, long packId)
        {
            if (packModel == null || string.IsNullOrWhiteSpace(packModel.Name) ||
                string.IsNullOrWhiteSpace(packModel.Description) || !packModel.IsDraft.HasValue)
            {
                throw new ArgumentException("Pack Model or IsDraft field was null");
            }

            DataValidation.ValidateId(packId);

            var entity = _dbContext.Set<Pack>().FirstOrDefault(x => x.Id == packId);
            DataValidation.ValidateEntity(entity, "Pack");

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
            DataValidation.ValidateEntity(entity, "Pack");

            _dbContext.Set<Pack>().Remove(entity);
            await _dbContext.SaveChangesAsync();
            _logger.LogInformation($"Pack id = {entity.Id} was deleted");
        }
    }
}
