using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.Extensions.Logging;
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

        public async Task<PackModel> CreatePack(PackModel packModel)
        {
            if (packModel == null || packModel.IsDraft == null)
            {
                throw new Exception("Pack Model or IsDraft field was null");
            }

            var packToSave = new Pack(packModel.Name, packModel.Description, packModel.IsDraft.Value);
            await _dbContext.AddAsync(packToSave);
            await _dbContext.SaveChangesAsync();
            _logger.LogInformation($"Pack id = {packToSave.Id} was saved");
            return new PackModel(packToSave.Id, packToSave.Name, packToSave.Description, packToSave.IsDraft);
        }

        public async Task AddToPack(AddToSetModel addToSetModel)
        {
            var pack = await PackById(addToSetModel.PackId);
            foreach (var questionId in addToSetModel.QuestionIds)
            {
                await AddQuestionToPack(pack, questionId);
            }

            this._dbContext.SaveChanges();

            string messageToLog = "Questions with id ";
            foreach (var questionId in addToSetModel.QuestionIds)
            {
                messageToLog += $"{questionId} ";
            }

            messageToLog += $"have been added to pack with id {pack.Id}";
            _logger.LogInformation(messageToLog);
        }

        public PackModel GetPack(long packId)
        {
            if (packId <= 0)
            {
                throw new Exception("Id was invalid number");
            }

            var entity = _dbContext.Packs.Where(x => x.Id == packId).Select(x => new
            {
                Id = x.Id,
                Name = x.Name,
                IsDraft = x.IsDraft,
                Descriptin = x.Description,
                Questions = x.Questions.Select(y => new Question
                {
                    Id = y.Id,
                    PackId = y.PackId,
                    QuestionText = y.QuestionText,
                    CorrectAnswers = y.CorrectAnswers.ToList()
                }).ToList()
            }).FirstOrDefault();

            var questionModels = new List<QuestionModel>();
            entity.Questions.ForEach(x =>
            {
                var correctAnswers = new List<string>();
                var entityAnswers = _dbContext.Set<CorrectAnswer>().Where(y => y.QuestionId == x.Id);
                entityAnswers.ToList().ForEach(y => correctAnswers.Add(y.AnswerText));
                questionModels.Add(new QuestionModel(x.Id, x.QuestionText, correctAnswers, x.PackId));
            });
            var result = new PackModel(entity.Id, entity.Name, entity.Descriptin, entity.IsDraft);
            result.Questions = questionModels;
            return result;
        }

        public async Task UpdatePack(PackModel packModel, long packId)
        {
            if (packId <= 0)
            {
                throw new Exception("Id was invalid number");
            }

            var entity = _dbContext.Set<Pack>().FirstOrDefault(x => x.Id == packId);
            if (entity == null)
            {
                throw new Exception("Pack is not found");
            }

            if (packModel.Name != string.Empty)
            {
                entity.Name = packModel.Name;
            }

            if (packModel.Description != string.Empty)
            {
                entity.Description = packModel.Description;
            }

            if (packModel.IsDraft != null)
            {
                entity.IsDraft = packModel.IsDraft.Value;
            }

            _dbContext.Set<Pack>().Update(entity);
            await _dbContext.SaveChangesAsync();
            _logger.LogInformation($"Pack id = {entity.Id} was updated");
        }

        public async Task DeletePack(long packId)
        {
            if (packId <= 0)
            {
                throw new Exception("Id was invalid number");
            }

            var entity = _dbContext.Set<Pack>().FirstOrDefault(x => x.Id == packId);
            if (entity == null)
            {
                throw new Exception("Pack is not found");
            }

            _dbContext.Set<Pack>().Remove(entity);
            await _dbContext.SaveChangesAsync();
            _logger.LogInformation($"Pack id = {entity.Id} was deleted");
        }

        private async Task<Pack> PackById(long packId)
        {            
            var pack = await _dbContext.Packs.FindAsync(packId)
                ?? throw new ArgumentException($"Pack with ID {packId} does not exist.");
            
            return pack;
        }

        private async Task AddQuestionToPack(Pack pack, long question)
        {
            var questionToAdd = await _dbContext.Questions.FindAsync(question);            
            if (questionToAdd != null && pack != null && !pack.Questions.Contains(questionToAdd))
            {
                pack.Questions.Add(questionToAdd);
            }
            else if (questionToAdd == null)
            {
                throw new ArgumentException("Question with given ID does not exist.");
            }            
        }
    }
}
