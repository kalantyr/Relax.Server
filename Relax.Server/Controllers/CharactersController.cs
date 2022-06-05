using Kalantyr.Web;
using Microsoft.AspNetCore.Mvc;
using Relax.Server.Services;

namespace Relax.Server.Controllers
{
    [ApiController]
    [Route("[controller]")]
    public class CharactersController: ControllerBase
    {
        private readonly CharactersService _charactersService;

        public CharactersController(CharactersService charactersService)
        {
            _charactersService = charactersService ?? throw new ArgumentNullException(nameof(charactersService));
        }

        [HttpGet]
        [Route("onlineCharacterIds")]
        public async Task<IActionResult> GetOnlineCharacterIdsAsync(CancellationToken cancellationToken)
        {
            var result = await _charactersService.GetOnlineCharacterIdsAsync(Request.GetAuthToken(), cancellationToken);
            return Ok(result);
        }

        [HttpPost]
        [Route("connect")]
        public async Task<IActionResult> ConnectAsync(uint characterId, CancellationToken cancellationToken)
        {
            var result = await _charactersService.ConnectAsync(characterId, Request.GetAuthToken(), cancellationToken);
            return Ok(result);
        }

        [HttpPost]
        [Route("disconnect")]
        public async Task<IActionResult> DisconnectAsync(CancellationToken cancellationToken)
        {
            var result = await _charactersService.DisconnectAsync(Request.GetAuthToken(), cancellationToken);
            return Ok(result);
        }
    }
}
