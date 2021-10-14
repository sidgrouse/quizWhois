using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using QuizWhois.Common.Models;
using QuizWhois.Domain.Services.Interfaces;

namespace QuizWhois.Api.Controllers
{
    [ApiController]
    [Route("[controller]")]
    public class QuizController
    {
        private readonly IQuizService _quizService;

        public QuizController(IQuizService quizService)
        {
            _quizService = quizService;           
        }

        // [Authorize]
        [HttpPost("{quizId}")]
        [ProducesResponseType(typeof(QuizModel),StatusCodes.Status200OK)]
        public async Task<ActionResult<QuizModel>> AddToSet(long quizId, QuestionModel questionModel)
        {
            var modifiedQuiz = await _quizService.AddToQuiz(quizId, questionModel.Id);
            return modifiedQuiz;
        }

        // [Authorize]
        [HttpPost("create")]
        [ProducesResponseType(typeof(QuizModel), StatusCodes.Status200OK)]
        public async Task<ActionResult<QuizModel>> CreateSet(List<long> questions, string quizName = "")
        {
            var formedQuiz = await _quizService.FormQuiz(questions, quizName);
            return formedQuiz;
        }
    }
}
