using System.Threading.Tasks;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using QuizWhois.Common.Models;
using QuizWhois.Domain.Services.Interfaces;

namespace QuizWhois.Api.Controllers
{
    [ApiController]
    [Route("pack")]
    public class PackController : Controller
    {
        private readonly IPackService _packService;

        public PackController(IPackService packService)
        {
            _packService = packService;
        }

        // [Authorize]
        [HttpPost]
        [ProducesResponseType(typeof(CreatedResult), StatusCodes.Status201Created)]
        public async Task<ActionResult<PackModelResponse>> CreatePack(PackModelRequest packModel)
        {
            var formedPack = await _packService.CreatePack(packModel);
            return new CreatedResult($"{Request.Scheme}://{Request.Host}{Request.Path}/{formedPack.Id}", formedPack);
        }

        // [Authorize]
        [HttpGet("{packId}")]
        [ProducesResponseType(typeof(PackModelResponse), StatusCodes.Status200OK)]
        [ProducesResponseType(StatusCodes.Status400BadRequest)]
        public ActionResult<PackModelResponse> GetPack(long packId)
        {
            return _packService.GetPack(packId);
        }

        // [Authorize]
        [HttpGet("/draft")]
        [ProducesResponseType(typeof(PacksModelResponse), StatusCodes.Status200OK)]
        [ProducesResponseType(StatusCodes.Status400BadRequest)]
        public ActionResult<PacksModelResponse> GetDraftPacks()
        {
            return _packService.GetPacks(true);
        }

        // [Authorize]
        [HttpGet]
        [ProducesResponseType(typeof(PacksModelResponse), StatusCodes.Status200OK)]
        [ProducesResponseType(StatusCodes.Status400BadRequest)]
        public ActionResult<PacksModelResponse> GetAll()
        {
            return _packService.GetPacks();
        }

        // [Authorize]
        [HttpPut("{packId}")]
        [ProducesResponseType(typeof(PackModelResponse), StatusCodes.Status200OK)]
        [ProducesResponseType(StatusCodes.Status400BadRequest)]
        public async Task<ActionResult> UpdatePack(PackModelRequest packModel, long packId)
        {
            await _packService.UpdatePack(packModel, packId);
            return Ok();
        }

        // [Authorize]
        [HttpDelete("{packId}")]
        [ProducesResponseType(typeof(PackModelResponse), StatusCodes.Status200OK)]
        [ProducesResponseType(StatusCodes.Status400BadRequest)]
        public async Task<ActionResult> DeletePack(long packId)
        {
            await _packService.DeletePack(packId);
            return Ok();
        }
    }
}
