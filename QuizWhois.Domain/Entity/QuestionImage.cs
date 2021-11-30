using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace QuizWhois.Domain.Entity
{
    public class QuestionImage
    {
        public long Id { get; set; }

        [Column(TypeName = "MediumBlob")]        
        public byte[] ImageData { get; set; }

        public string Name { get; set; }

        public string Caption { get; set; }

        public long QuestionId { get; set; }

        public Question Question { get; set; }       
    }
}
