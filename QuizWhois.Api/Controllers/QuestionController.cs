using System.Collections.Generic;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using QuizWhois.Api.Models;
using QuizWhois.Common.Models;
using QuizWhois.Domain.Services.Interfaces;

namespace QuizWhois.Api.Controllers
{
    [ApiController]
    [Route("question")]
    public class QuestionController : Controller
    {
        private readonly IQuestionService _questionService;
        private readonly IQuestionRatingService _questionRatingService;

        public QuestionController(IQuestionService questionService, IQuestionRatingService questionRatingService)
        {
            _questionService = questionService;
            _questionRatingService = questionRatingService;
        }
        
        [HttpPost]
        [ProducesResponseType(StatusCodes.Status200OK)]
        [ProducesResponseType(StatusCodes.Status400BadRequest)]
        public async Task<ActionResult> CreateQuestions(List<QuestionModel> questions)
        {
            await _questionService.CreateQuestions(questions);
            return Ok();
        }

        [HttpPost("{questionId}/rating")]
        [ProducesResponseType(StatusCodes.Status200OK)]
        [ProducesResponseType(StatusCodes.Status400BadRequest)]
        public ActionResult<QuestionRatingModel> Post(long questionId, QuestionRatingRequest request)
        {
            var addedRating = _questionRatingService.AddRating(questionId, request.UserId, request.Mark);
            return Ok(addedRating);
        }

        [HttpPut("{questionId}/rating")]
        [ProducesResponseType(StatusCodes.Status200OK)]
        [ProducesResponseType(StatusCodes.Status400BadRequest)]
        public ActionResult<QuestionRatingModel> Update(long questionId, QuestionRatingRequest request)
        {
            var updatedRating = _questionRatingService.UpdateRating(questionId, request.UserId, request.Mark);
            return Ok(updatedRating);
        }

        [HttpDelete("{questionId}/rating")]
        [ProducesResponseType(StatusCodes.Status200OK)]
        [ProducesResponseType(StatusCodes.Status400BadRequest)]
        public IActionResult Delete(long questionId, QuestionRatingRequest request)
        {
            _questionRatingService.DeleteRating(questionId, request.UserId);
            return Ok();
        }

        [Authorize]
        [HttpGet]
        [ProducesResponseType(StatusCodes.Status200OK)]
        [ProducesResponseType(StatusCodes.Status400BadRequest)]
        [Route("{questionId}/average")]
        public ActionResult<double> GetAverage(long questionId)
        {
            var averageRating = _questionRatingService.GetAverageRating(questionId);
            return averageRating;
        }
    }
}
