using System.Threading.Tasks;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using QuizWhois.Common.Models;
using QuizWhois.Domain.Services.Interfaces;

namespace QuizWhois.Api.Controllers
{
    [ApiController]
    [Route("hint")]
    public class HintController : Controller
    {
        private readonly IHintService _hintService;

        public HintController(IHintService hintService)
        {
            _hintService = hintService;
        }

        [HttpPost]
        [ProducesResponseType(StatusCodes.Status200OK)]
        [ProducesResponseType(StatusCodes.Status400BadRequest)]
        public Task<HintModel> Post(AddHintModel model)
        {
            var addedHint = _hintService.AddHint(model);
            return addedHint;
        }
    }
}
