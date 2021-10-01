using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Domain.Entity
{
    public class Question : Base.Base
    {
        public string QuestionText { get; set; }
        public string Answer { get; set; }
    }
}
