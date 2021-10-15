using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.EntityFrameworkCore;
using QuizWhois.Common.Models;
using QuizWhois.Domain.Database;
using QuizWhois.Domain.Entity;
using QuizWhois.Domain.Services.Interfaces;

namespace QuizWhois.Domain.Services.Implementations
{
    public class QuizService : IQuizService
    {
        private readonly ApplicationContext _db;

        public QuizService(ApplicationContext context)
        {
            _db = context;
        }

        public async Task<QuizModel> FormQuiz(List<long> questions, string quizName = "")
        {
            var quizToSave = new Quiz(quizName);

            foreach (var question in questions)
            {
                await AddToQuiz(quizToSave, question);
            }

            await _db.AddAsync(quizToSave);
            await _db.SaveChangesAsync();
            return new QuizModel(quizToSave.Id, quizToSave.Questions.Select(x => x.Id), quizToSave.Name);
        }

        public async Task AddToQuiz(AddToSetModel[] addToSet)
        {
            for (var i = 0; i < addToSet.Length; i++)
            {
                await AddToQuiz(await QuizById(addToSet[i].QuizId), addToSet[i].QuestionId);
            }

            this._db.SaveChanges();
        }

        private async Task<Quiz> QuizById(long quizId)
        {            
            var quiz = await _db.Set<Quiz>().FindAsync(quizId);                     
            if (quiz == null)
            {
                throw new System.NullReferenceException("Quiz with given ID does not exist.");
            }

            return quiz;
        }

        private async Task AddToQuiz(Quiz quiz, long question)
        {
            var questionToAdd = await _db.Set<Question>().FindAsync(question);            
            if (questionToAdd != null && quiz != null && !quiz.Questions.Contains(questionToAdd))
            {
                quiz.Questions.Add(questionToAdd);
            }
            else if (questionToAdd == null)
            {
                throw new System.NullReferenceException("Question with given ID does not exist.");
            }            
        }
    }
}
