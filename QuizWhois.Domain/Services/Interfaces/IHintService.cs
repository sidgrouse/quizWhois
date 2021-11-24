using System.Threading.Tasks;
using QuizWhois.Common.Models;

namespace QuizWhois.Domain.Services.Interfaces
{
    public interface IHintService
    {
        public Task<HintModel> AddHint(AddHintModel model);
    }
}
