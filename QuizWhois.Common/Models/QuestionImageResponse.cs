using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace QuizWhois.Common.Models
{
    public class QuestionImageResponse
    {
        public byte[] ImageData { get; set; }

        public string Name { get; set; }

        public string Caption { get; set; }

        public long QuestionId { get; set; }
    }
}
