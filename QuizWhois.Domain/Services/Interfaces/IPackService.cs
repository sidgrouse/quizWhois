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
        public Task AddToPack(AddToSetModel addToSetModel);

        public Task<PackModel> CreatePack(PackModel packModel);

        public PackModel GetPack(long packId);

        public Task UpdatePack(PackModel packModel, long packId);

        public Task DeletePack(long packId);
    }
}
