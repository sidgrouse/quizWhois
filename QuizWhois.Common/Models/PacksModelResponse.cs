using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace QuizWhois.Common.Models
{
    public class PacksModelResponse
    {
        public List<PackModelResponse> Packs { get; set; }

        public PacksModelResponse(List<PackModelResponse> packs)
        {
            Packs = packs;
        }
    }
}
