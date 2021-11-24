using System.Collections.Generic;
using System.Linq;
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
        private readonly IQuestionImageService _questionImageService;

        public QuestionController(IQuestionService questionService, IQuestionRatingService questionRatingService, IQuestionImageService questionImageService)
        {
            _questionService = questionService;
            _questionRatingService = questionRatingService;
            _questionImageService = questionImageService;
        }
        
        [HttpPost]
        [ProducesResponseType(typeof(CreatedResult), StatusCodes.Status201Created)]
        [ProducesResponseType(StatusCodes.Status400BadRequest)]
        public async Task<ActionResult> CreateQuestions(QuestionsCreatingModelRequest questions)
        {
            var formedQuestions = await _questionService.CreateQuestions(questions);
            if (formedQuestions.Questions.Count == 1)
            {
                return new CreatedResult(
                    $"{Request.Scheme}://{Request.Host}{Request.Path}/{formedQuestions.Questions.FirstOrDefault().Id}",
                    formedQuestions);
            }
            else
            {
                return new CreatedResult($"{Request.Scheme}://{Request.Host}{Request.Path}", formedQuestions);
            }
        }

        [HttpGet("{questionId}")]
        [ProducesResponseType(typeof(QuestionModelResponse), StatusCodes.Status200OK)]
        [ProducesResponseType(StatusCodes.Status400BadRequest)]
        public ActionResult<QuestionModelResponse> GetQuestion(long questionId)
        {
            return _questionService.GetQuestion(questionId);
        }

        [HttpPut("{questionId}")]
        [ProducesResponseType(typeof(QuestionRatingModel), StatusCodes.Status200OK)]
        [ProducesResponseType(StatusCodes.Status400BadRequest)]
        public async Task<ActionResult> Update(QuestionModelRequest questionModel, long questionId)
        {
            await _questionService.UpdateQuestion(questionModel, questionId);
            return Ok();
        }

        [HttpDelete("{questionId}")]
        [ProducesResponseType(typeof(QuestionRatingModel), StatusCodes.Status200OK)]
        [ProducesResponseType(StatusCodes.Status400BadRequest)]
        public async Task<ActionResult> Delete(long questionId)
        {
            await _questionService.DeleteQuestion(questionId);
            return Ok();
        }

        [HttpPost("{questionId}/rating")]
        [ProducesResponseType(typeof(QuestionRatingModel), StatusCodes.Status200OK)]
        [ProducesResponseType(StatusCodes.Status400BadRequest)]
        public ActionResult<QuestionRatingModel> Post(long questionId, QuestionRatingRequest request)
        {
            var addedRating = _questionRatingService.AddRating(questionId, request.UserId, request.Mark);
            return Ok(addedRating);
        }

        [HttpPut("{questionId}/rating")]
        [ProducesResponseType(typeof(QuestionRatingModel), StatusCodes.Status200OK)]
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

        [HttpGet]
        [ProducesResponseType(typeof(double), StatusCodes.Status200OK)]
        [ProducesResponseType(StatusCodes.Status400BadRequest)]
        [Route("{questionId}/average")]
        public ActionResult<double> GetAverage(long questionId)
        {
            var averageRating = _questionRatingService.GetAverageRating(questionId);
            return averageRating;
        }

        [HttpPost("{questionId}/image")]
        [ProducesResponseType(StatusCodes.Status200OK)]
        [ProducesResponseType(StatusCodes.Status400BadRequest)]
        public ActionResult Post(long questionId, IFormFile image)
        {
            var addedImage = _questionImageService.AddOrReplaceImage(questionId, image);
            return Ok();
        }

        [HttpDelete("{questionId}/image")]
        [ProducesResponseType(StatusCodes.Status200OK)]
        [ProducesResponseType(StatusCodes.Status400BadRequest)]
        public ActionResult DeleteImage(long questionId)
        {
            var deleteImage = _questionImageService.DeleteImage(questionId);
            return Ok();
        }
    }
}
