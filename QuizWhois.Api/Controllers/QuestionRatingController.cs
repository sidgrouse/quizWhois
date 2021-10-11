using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using QuizWhois.Common.Models;
using QuizWhois.Domain.Services.Interfaces;

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
        public ActionResult<QuestionRatingModel> Post(long questionModelId, long userId, uint rating)
        {
            var addedRating = _questionRatingService.AddRating(questionModelId, userId, rating);
            return Ok(addedRating);
        }

        [HttpPut]
        [ProducesResponseType(StatusCodes.Status200OK)]
        [ProducesResponseType(StatusCodes.Status400BadRequest)]
        public ActionResult<QuestionRatingModel> Update(long questionModelId, long userId, uint rating)
        {
            var updatingRating = _questionRatingService.UpdateRating(questionModelId, userId, rating);
            return Ok(updatingRating);
        }

        [HttpDelete]
        [ProducesResponseType(StatusCodes.Status200OK)]
        [ProducesResponseType(StatusCodes.Status400BadRequest)]
        public IActionResult Delete(long questionModelId, long userId)
        {
            _questionRatingService.DeleteRating(questionModelId, userId);
            return Ok();
        }

        [HttpGet]
        [ProducesResponseType(StatusCodes.Status200OK)]
        [ProducesResponseType(StatusCodes.Status400BadRequest)]
        [Route("getaverage")]
        public ActionResult<double> GetAverage(long questionId)
        {
            var averageRating = _questionRatingService.GetAverageRating(questionId);
            return Ok(averageRating);
        }
    }
}
