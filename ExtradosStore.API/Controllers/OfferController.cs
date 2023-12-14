using ExtradosStore.Common.CustomExceptions.PostExceptions;
using ExtradosStore.Common.CustomRequest.OfferRequest;
using ExtradosStore.Services.Interfaces;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

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

            try
            {
                var rowsAffected = await _offerService.CreateOfferService(createOfferRequest);
                return Ok("offer created");
            }
            catch (DuplicateNameCategoryException ex)
            {
                Console.WriteLine(ex.Message);
                return Conflict("the name offer is already in use");
            }
            catch (Exception Ex)
            {
                Console.WriteLine($"Error creating a new offer:  {Ex.Message} {Ex.StackTrace}");

                return StatusCode(500, "server error:");
            }

        }


    }
}
