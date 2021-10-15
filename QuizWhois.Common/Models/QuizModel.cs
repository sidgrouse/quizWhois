using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace QuizWhois.Common.Models
{
    public class QuizModel
    {
        public QuizModel(long id, IEnumerable<long> questions, string name = "")
        {
            Id = id;
            Questions = questions.ToList();
            Name = string.IsNullOrEmpty(name) ? id.ToString() : name;
        }

        public long Id { get; set; }

        public List<long> Questions { get; set; } = new List<long>();

        public string Name { get; set; }
    }
}
