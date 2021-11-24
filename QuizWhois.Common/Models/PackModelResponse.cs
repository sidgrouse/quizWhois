using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace QuizWhois.Common.Models
{
    public class PackModelResponse
    {
        public PackModelResponse(long id, string name, string description, bool? isDraft)
        {
            Id = id;
            Name = name;
            Description = description;
            IsDraft = isDraft;
        }

        public long Id { get; set; }

        public IEnumerable<QuestionModelResponse> Questions { get; set; }

        public string Name { get; set; }

        public string Description { get; set; }

        public bool? IsDraft { get; set; }
    }
}
