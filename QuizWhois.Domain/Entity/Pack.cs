using System.Collections.Generic;

namespace QuizWhois.Domain.Entity
{
    public class Pack
    {
        public Pack(string name, string description, bool isDraft)
        {
            Name = name;
            Description = description;
            IsDraft = isDraft;
        }

        public long Id { get; set; }

        public List<Question> Questions { get; set; } = new List<Question>();

        public string Name { get; set; }

        public string Description { get; set; }

        public bool IsDraft { get; set; }
    }
}
