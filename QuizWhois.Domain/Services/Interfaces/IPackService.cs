using System.Threading.Tasks;
using QuizWhois.Common.Models;

namespace QuizWhois.Domain.Services.Interfaces
{
    public interface IPackService
    {
        public Task<PackModelResponse> CreatePack(PackModelRequest packModel);

        public PackModelResponse GetPack(long packId);

        public SomePacksModelResponse GetAll(bool? isDraft = null);

        public Task UpdatePack(PackModelRequest packModel, long packId);

        public Task DeletePack(long packId);
    }
}
