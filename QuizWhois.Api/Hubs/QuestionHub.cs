using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.SignalR;
using QuizWhois.Common.Models;
using QuizWhois.Domain.Services.Interfaces;

namespace QuizWhois.Api.Hubs
{
    public class QuestionHub: Hub
    {
        private readonly IQuestionService _questionService;
        public QuestionHub(IQuestionService questionService)
        {
            _questionService = questionService;
        }
        public async Task SendQuestion()
        {
            var addedOperation = _questionService.GetRandomQuestion();
            await Clients.Caller.SendAsync("ReceiveQuestion", addedOperation.Id, addedOperation.QuestionText);

        }
        public async Task SendAnswer(long id, string questionText, string questionAnswer) //QuestionModel questionModel
        {
            if (questionText == string.Empty || questionAnswer == string.Empty)
            {
                throw new Exception("questionText ore questionAnswer in QuestionHub.SendAnswer is null");
            }
            var isAnswerRight = _questionService.CheckAnswer(new QuestionModel(id, questionText, questionAnswer));
            await Clients.Caller.SendAsync("ReceiveAnswer", id, isAnswerRight);

        }


    }
    
}
