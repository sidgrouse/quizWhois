using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using QuizWhois.Common.Models;
using QuizWhois.Domain.Database;
using QuizWhois.Domain.Entity;
using QuizWhois.Domain.Services.Interfaces;

namespace QuizWhois.Domain.Services.Implementations
{
    public class QuizService : IQuizService
    {
        private readonly ApplicationContext _dbContext;

        public QuizService(ApplicationContext context)
        {
            _dbContext = context;
        }

        public async Task<QuizModel> CreateQuiz(List<long> questionIds, string quizName = "")
        {
            var quizToSave = new Quiz(quizName);

            foreach (var question in questionIds)
            {
                await AddQuestionToQuiz(quizToSave, question);
            }

            await _dbContext.AddAsync(quizToSave);
            await _dbContext.SaveChangesAsync();
            var questions = quizToSave.Questions.Select(q => new QuestionModel(q.Id, q.QuestionText, q.CorrectAnswer));
            return new QuizModel(quizToSave.Id, questions, quizToSave.Name);
        }

        public async Task AddToQuiz(AddToSetModel addToSetModel)
        {
            var quiz = await QuizById(addToSetModel.QuizId);
            foreach (var questionId in addToSetModel.QuestionIds)
            {
                await AddQuestionToQuiz(quiz, questionId);
            }

            this._dbContext.SaveChanges();
        }

        private async Task<Quiz> QuizById(long quizId)
        {            
            var quiz = await _dbContext.Quizzes.FindAsync(quizId)
                ?? throw new System.ArgumentException($"Quiz with ID {quizId} does not exist.");
            
            return quiz;
        }

        private async Task AddQuestionToQuiz(Quiz quiz, long question)
        {
            var questionToAdd = await _dbContext.Questions.FindAsync(question);            
            if (questionToAdd != null && quiz != null && !quiz.Questions.Contains(questionToAdd))
            {
                quiz.Questions.Add(questionToAdd);
            }
            else if (questionToAdd == null)
            {
                throw new System.ArgumentException("Question with given ID does not exist.");
            }            
        }
    }
}
