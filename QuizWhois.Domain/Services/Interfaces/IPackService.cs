using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using QuizWhois.Common.Models;

namespace QuizWhois.Domain.Services.Interfaces
{
    public interface IPackService
    {
        public Task<PackModelResponse> CreatePack(PackModelRequest packModel);

        public PackModelResponse GetPack(long packId);

        public Task UpdatePack(PackModelRequest packModel, long packId);

        public Task DeletePack(long packId);
    }
}
