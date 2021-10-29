using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace QuizWhois.Common.Models
{
    public class QuizModel
    {
        public QuizModel(long id, IEnumerable<QuestionModel> questions, string name)
        {
            Id = id;
            Questions = questions;
            Name = name;
        }

        public long Id { get; set; }

        public IEnumerable<QuestionModel> Questions { get; set; }

        public string Name { get; set; }
    }
}
