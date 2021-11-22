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

        public string Caption { get; set; }

        public int QuestionId { get; set; }

        public Question Question { get; set; }
    }
}
