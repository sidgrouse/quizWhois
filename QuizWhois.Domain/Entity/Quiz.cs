using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace QuizWhois.Domain.Entity
{
    public class Quiz
    {
        public Quiz(string name = "")
        {
            Name = name;
        }

        public long Id { get; set; }

        public List<Question> Questions { get; set; } = new List<Question>();

        public string Name { get; set; }
    }
}
