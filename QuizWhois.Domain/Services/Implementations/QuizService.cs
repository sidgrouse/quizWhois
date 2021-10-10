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
        private ApplicationContext _db { get; set; }

        public QuizService(ApplicationContext context)
        {
            _db = context;
        }


        public async Task<QuizModel> FormQuiz(List<long> questions, string quizName = "")
        {
            var quizToSave = new Quiz(quizName);
            var questionsOfQuiz = _db.Set<Question>().Where(x => questions.Contains(x.Id));
            quizToSave.Questions = questionsOfQuiz.ToList();
            _db.Set<Quiz>().Add(quizToSave);
            await _db.SaveChangesAsync();
            return new QuizModel(quizToSave.Id, quizToSave.Questions.Select(x => x.Id));
        }

        public async Task<QuizModel> AddToQuiz(long quizId, Question question)
        {
            var quiz = await _db.Quizzes.FirstOrDefaultAsync(x => x.Id == quizId);
            if (quiz == null)
            {
                quiz = new Quiz();
                _db.Add(quiz);
            }

            quiz.Questions.Add(question);
            _db.SaveChanges();
            return new QuizModel(quiz.Id, quiz.Questions.Select(x => x.Id), quiz.Name);
        }

        public async Task<QuizModel> AddToQuiz(long quizId, long question)
        {
            var questionToAdd = await _db.Set<Question>().FindAsync(question);
            var quiz = await _db.Quizzes.FirstOrDefaultAsync(x => x.Id == quizId);
            if (quiz == null)
            {
                quiz = new Quiz();
                _db.Add(quiz);
            }

            if (questionToAdd == null)
            {
                // something bad happened
            }
            
            quiz.Questions.Add(questionToAdd);
            _db.SaveChanges();
            return new QuizModel(quiz.Id, quiz.Questions.Select(x => x.Id), quiz.Name);
        }
    }
}
