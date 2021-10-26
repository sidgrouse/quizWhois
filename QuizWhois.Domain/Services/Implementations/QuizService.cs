using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Logging;
using QuizWhois.Common.Models;
using QuizWhois.Domain.Database;
using QuizWhois.Domain.Entity;
using QuizWhois.Domain.Services.Interfaces;

namespace QuizWhois.Domain.Services.Implementations
{
    public class QuizService : IQuizService
    {
        private readonly ApplicationContext _db;
        private readonly ILogger<QuizService> _logger;

        public QuizService(ApplicationContext context, ILogger<QuizService> logger)
        {
            _db = context;
            _logger = logger;
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
            _logger.LogInformation($"Quiz id = {quizToSave.Id} was saved");
            return new QuizModel(quizToSave.Id, quizToSave.Questions.Select(x => x.Id), quizToSave.Name);
        }

        public async Task AddToQuiz(AddToSetModel[] addToSet)
        {
            var mutableQuizIds = new List<long>();
            var mutableQuestionIds = new List<long>();
            for (var i = 0; i < addToSet.Length; i++)
            {
                await AddToQuiz(await QuizById(addToSet[i].QuizId), addToSet[i].QuestionId);
                if (!mutableQuizIds.Contains(addToSet[i].QuizId))
                {
                    mutableQuizIds.Add(addToSet[i].QuizId);
                }

                if (!mutableQuestionIds.Contains(addToSet[i].QuestionId))
                {
                    mutableQuestionIds.Add(addToSet[i].QuestionId);
                }
            }

            this._db.SaveChanges();
            string messageToLog = "Questions with id ";

            foreach (var quizId in mutableQuizIds)
            {
                messageToLog += $"{quizId} ";
            }

            messageToLog += "have been added to quizes with id";

            foreach (var questionId in mutableQuestionIds)
            {
                messageToLog += $"{questionId} ";
            }

            _logger.LogInformation(messageToLog);
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
