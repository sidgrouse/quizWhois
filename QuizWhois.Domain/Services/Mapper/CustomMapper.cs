using System.Collections.Generic;
using QuizWhois.Common.Models;
using QuizWhois.Domain.Entity;

namespace QuizWhois.Domain.Services.Mapper
{
    public static class CustomMapper
    {
        public static QuestionModelResponse ToQuestionModelResponse(this Question question)
        {
            if (question == null)
            {
                return null;
            }

            var answers = new List<string>();
            question.CorrectAnswers.ForEach(x => answers.Add(x.AnswerText));
            return new QuestionModelResponse(question.Id, question.QuestionText, answers, question.PackId);
        }

        public static PackModelResponse ToPackModelResponse(this Pack pack)
        {
            if (pack == null)
            {
                return null;
            }

            var question = new List<QuestionModelResponse>();
            pack.Questions.ForEach(x => question.Add(x.ToQuestionModelResponse()));
            var result = new PackModelResponse(pack.Id, pack.Name, pack.Description, pack.IsDraft);
            result.Questions = question;
            return result;
        }
    }
}
