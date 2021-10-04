using Microsoft.AspNetCore.Mvc;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using QuizWhois.Domain.Services.Interfaces;
using Microsoft.AspNetCore.Http;
using QuizWhois.Domain.Database;
using QuizWhois.Domain.Entity;
using QuizWhois.Common.Models;

namespace QuizWhois.Api.Controllers
{
    [ApiController]
    [Route("[controller]")]
    public class QuestionController : Controller
    {
        private readonly IQuestionService _questionService;

        public QuestionController(IQuestionService questionService)
        {
            _questionService = questionService;
        }

        [HttpPost]
        [ProducesResponseType(StatusCodes.Status200OK)]
        [ProducesResponseType(StatusCodes.Status400BadRequest)]
        public IActionResult Post(QuestionModel questionModel)
        {
            var addedOperation = _questionService.AddQuestion(questionModel);
            return Ok(addedOperation);
        }
    }
}
