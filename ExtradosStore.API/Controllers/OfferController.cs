using ExtradosStore.Common.CustomRequest.OfferRequest;
using ExtradosStore.Services.Interfaces;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using System.Security.Claims;

namespace ExtradosStore.API.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class OfferController : ControllerBase
    {
        private readonly IOfferService _offerService;
        public OfferController(IOfferService offerService)
        {
            _offerService = offerService;
        }




        [HttpPost("create")]
        [Authorize(Roles = "admin, user")]

        public async Task<IActionResult> CreateNewOffer([FromBody] CreateOfferRequest createOfferRequest)
        {
            var userIdClaim = User.Claims.FirstOrDefault(c => c.Type == ClaimTypes.NameIdentifier);
            if (userIdClaim == null) return StatusCode(401, "Unauthorized");
            int.TryParse(userIdClaim.Value, out int userId);
            var rowsAffected = await _offerService.CreateOfferService(createOfferRequest, userId);
            return Ok("offer created");
        }
        [HttpGet("getallactive")]
        [Authorize(Roles = "admin,user")]

        public async Task<IActionResult> GetAllOfferActive()
        {
            var rowsAffected = await _offerService.GetAllOfferActive();
            return Ok(rowsAffected);
        }

        [HttpGet("getall")]
        [Authorize(Roles = "admin")]

        public async Task<IActionResult> GetAllOffer()
        {
            var rowsAffected = await _offerService.GetAllOffer();
            return Ok(rowsAffected);

        }


    }
}
