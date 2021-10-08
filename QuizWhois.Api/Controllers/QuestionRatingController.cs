using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using QuizWhois.Common.Models;
using QuizWhois.Domain.Services.Interfaces;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace QuizWhois.Api.Controllers
{
    [ApiController]
    [Route("[controller]")]
    public class QuestionRatingController : Controller
    {
        private readonly IQuestionRatingService _questionRatingService;

        public QuestionRatingController(IQuestionRatingService questionRatingService)
        {
            _questionRatingService = questionRatingService;
        }

        [HttpPost]
        [ProducesResponseType(StatusCodes.Status200OK)]
        [ProducesResponseType(StatusCodes.Status400BadRequest)]
        public IActionResult Post(QuestionModel questionModel, long userId, uint rating)
        {
            var addedRating = _questionRatingService.AddRating(questionModel, userId, rating);
            return Ok(addedRating);
        }

        [HttpPut]
        [ProducesResponseType(StatusCodes.Status200OK)]
        [ProducesResponseType(StatusCodes.Status400BadRequest)]
        public IActionResult Update(QuestionModel questionModel, long userId, uint rating)
        {
            var updatingRating = _questionRatingService.UpdateRating(questionModel, userId, rating);
            return Ok(updatingRating);
        }

        [HttpDelete]
        [ProducesResponseType(StatusCodes.Status200OK)]
        [ProducesResponseType(StatusCodes.Status400BadRequest)]
        public IActionResult Delete(QuestionModel questionModel, long userId)
        {
            _questionRatingService.DeleteRating(questionModel, userId);
            return Ok();
        }

        [HttpGet]
        [ProducesResponseType(StatusCodes.Status200OK)]
        [ProducesResponseType(StatusCodes.Status400BadRequest)]
        [Route("getaverage")]
        public IActionResult GetAverage(long questionId)
        {
            var averageRating = _questionRatingService.GetAverageRating(questionId);
            return Ok(averageRating);
        }
    }
}
