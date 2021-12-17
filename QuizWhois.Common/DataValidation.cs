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

        public static void ValidateEntity(object entity, string entityName)
        {
            if (entity == null)
            {
                throw new ArgumentException($"{entityName} with such id doesn't exist");
            }
        }
    }
}
