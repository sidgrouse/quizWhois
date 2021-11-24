using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace QuizWhois.Domain.Entity
{
    public class QuestionImage
    {
        public long QuestionImageId { get; set; }

        public byte[] ImageData { get; set; }

        public string Name { get; set; }

        public string Caption { get; set; }

        public long QuestionId { get; set; }

        public Question Question { get; set; }

        public QuestionImage(string name, byte[] data, string caption, long questionId)
        {
            Name = name;
            Caption = caption;
            ImageData = data;
            QuestionId = questionId;
        }
    }
}
