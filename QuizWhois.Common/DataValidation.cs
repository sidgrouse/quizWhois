using System;

namespace QuizWhois.Common
{
    public static class DataValidation
    {
        public static void ValidateId(long id)
        {
            if (id <= 0)
            {
                throw new ArgumentException("Id was invalid number");
            }
        }
    }
}
