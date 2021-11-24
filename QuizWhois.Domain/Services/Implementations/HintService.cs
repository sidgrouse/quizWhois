using System;
using System.Threading.Tasks;
using QuizWhois.Common.Models;
using QuizWhois.Domain.Database;
using QuizWhois.Domain.Entity;
using QuizWhois.Domain.Services.Interfaces;

namespace QuizWhois.Domain.Services.Implementations
{
    public class HintService : IHintService
    {
        private ApplicationContext Context { get; set; }

        public HintService(ApplicationContext context)
        {
            Context = context;
        }

        public async Task<HintModel> AddHint(AddHintModel model)
        {
            if (model.QuestionId <= 0 || model.Text is null || model.Text == string.Empty)
            {
                throw new ArgumentException("hintModel in HintService.AddHint was null");
            }

            var entity = new Hint(model.QuestionId, model.Text);
            Context.Set<Hint>().Add(entity);
            await Context.SaveChangesAsync();
            return new HintModel(entity.Id, entity.QuestionId, entity.Text);
        }
    }
}
