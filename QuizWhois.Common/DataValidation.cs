using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace QuizWhois.Common
{
    public static class DataValidation
    {
        public static void ValidateId(long id)
        {
            if (id <= 0)
            {
                throw new Exception("Id was invalid number");
            }
        }
    }
}
