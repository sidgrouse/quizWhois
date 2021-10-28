﻿using System;
using QuizWhois.Common.Models;
using QuizWhois.Domain.Database;
using QuizWhois.Domain.Entity;
using QuizWhois.Domain.Services.Interfaces;

namespace QuizWhois.Domain.Services.Implementations
{
    public class HintService : IHintService
    {
        public ApplicationContext Context { get; set; }

        public HintService(ApplicationContext context)
        {
            Context = context;
        }

        public HintModel AddHint(long questionId, string text)
        {
            if (questionId <= 0 || text is null || text == string.Empty)
            {
                throw new ArgumentNullException("hintModel in HintService.AddHint was null");
            }

            var entity = new Hint(questionId, text);
            Context.Set<Hint>().Add(entity);
            Context.SaveChanges();
            return new HintModel(entity.Id, entity.QuestionId, entity.Text);
        }
    }
}