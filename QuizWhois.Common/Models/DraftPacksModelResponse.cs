using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace QuizWhois.Common.Models
{
    public class DraftPacksModelResponse
    {
        public List<PackModelResponse> DraftPacks { get; set; }

        public DraftPacksModelResponse(List<PackModelResponse> draftPacks)
        {
            DraftPacks = draftPacks;
        }
    }
}
