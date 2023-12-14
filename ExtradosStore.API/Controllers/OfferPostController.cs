using ExtradosStore.Common.CustomRequest.OfferRequest;
using ExtradosStore.Services.Interfaces;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace ExtradosStore.API.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class OfferPostController : ControllerBase
    {
        private readonly IOfferPostService _offerPostService;
        public OfferPostController(IOfferPostService offerPostService)
        {
            _offerPostService = offerPostService;
        }


        [HttpPost("create")]
        [Authorize(Roles = "admin, user")]

        public async Task<IActionResult> AddPostToOffer([FromBody] AddPostToOfferRequest addPostToOfferRequest)
        {
            try
            {
                var rowsAffected = await _offerPostService.AddPostToOfferService(addPostToOfferRequest);
                if (rowsAffected == 0) return StatusCode(500, "testeando servicio");
                return Ok("product added to offer");
            }
            catch (Exception ex)
            {

                Console.WriteLine($"error:  {ex}");
                Console.WriteLine($"stack trace: {ex.StackTrace}");
                return StatusCode(500, "server error");
            }
        }
    }
}
