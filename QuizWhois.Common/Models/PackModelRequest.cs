using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace QuizWhois.Common.Models
{
    public class PackModelRequest
    {
        public PackModelRequest(string name, string description, bool? isDraft)
        {
            Name = name;
            Description = description;
            IsDraft = isDraft;
        }

        public string Name { get; set; }

        public string Description { get; set; }

        public bool? IsDraft { get; set; }
    }
}
