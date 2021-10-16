using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.SignalR;
using QuizWhois.Common.Models;
using QuizWhois.Domain.Services.Interfaces;

namespace QuizWhois.Api.Hubs
{
    public class QuestionHub : Hub
    {
        private readonly IUserAnswerService _userAnswerService;

        public QuestionHub(IUserAnswerService userAnswerService)
        {
            _userAnswerService = userAnswerService;
        }

        public async Task SendQuestion()
        {
            var addedOperation = _userAnswerService.GetRandomQuestion();
            await Clients.Caller.SendAsync("ReceiveQuestion", addedOperation.Id, addedOperation.QuestionText);
        }

        public async Task SendAnswer(long id, string userAnswer)
        {
            if (userAnswer == string.Empty)
            {
                throw new ArgumentException("userAnswer in QuestionHub.SendAnswer is empty");
            }

            var isAnswerRight = _userAnswerService.CheckAnswer(new UserAnswerModel(id, userAnswer));
            await Clients.Caller.SendAsync("ReceiveAnswer", id, isAnswerRight);
        }
    }    
}
