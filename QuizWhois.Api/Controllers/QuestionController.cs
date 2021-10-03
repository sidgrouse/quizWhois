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
        private readonly ApplicationContext _applicationContext;

        public QuestionController(IQuestionService questionService, ApplicationContext applicationContext)
        {
            _questionService = questionService;
            _applicationContext = applicationContext;
        }

        [ProducesResponseType(StatusCodes.Status200OK)]
        [HttpGet]
        [Route("getall")]
        public JsonResult Get()
        {
            return new JsonResult(_applicationContext.Set<Question>().ToList());
        }

        [HttpPost]
        [ProducesResponseType(StatusCodes.Status200OK)]
        [ProducesResponseType(StatusCodes.Status400BadRequest)]
        public JsonResult Post(QuestionModel questionModel)
        {
            _questionService.AddQuestion(questionModel);
            return new JsonResult("Operation was successfully added");
        }
    }
}
