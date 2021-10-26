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
        [HttpPost("questions")]
        [ProducesResponseType(typeof(QuizModel), StatusCodes.Status200OK)]
        public async Task<ActionResult> AddToSet([FromBody] AddToSetModel model)
        {
            await _quizService.AddToQuiz(model);
            return new OkResult();
        }

        // [Authorize]
        [HttpPost]
        [ProducesResponseType(typeof(QuizModel), StatusCodes.Status200OK)]
        public async Task<ActionResult<QuizModel>> CreateSet(List<long> questions, string quizName = "")
        {
            var formedQuiz = await _quizService.CreateQuiz(questions, quizName);
            return formedQuiz;
        }
    }
}
