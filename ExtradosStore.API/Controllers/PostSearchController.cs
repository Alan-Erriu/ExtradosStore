using ExtradosStore.Services.Interfaces;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace ExtradosStore.API.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class PostSearchController : ControllerBase
    {
        private readonly IPostSearchService _postSearchService;
        public PostSearchController(IPostSearchService postSearchService)
        {
            _postSearchService = postSearchService;

        }
        [HttpGet("getallactive")]
        [Authorize(Roles = "admin, user")]
        public async Task<IActionResult> GetAllActivePostsWithNoOfferOrExpiredOffer()
        {
            try
            {

                var allPostActive = await _postSearchService.GetAllActivePostsWithNoOfferOrExpiredOffer();
                return Ok(allPostActive);

            }
            catch (Exception ex)
            {
                Console.WriteLine($"Error getting all post: {ex.Message} {ex.StackTrace}");
                return StatusCode(500, "server error");
            }
        }
        [HttpGet("getallactivewithoffer")]
        [Authorize(Roles = "admin, user")]
        public async Task<IActionResult> GetAllPostActiveWithOffer()
        {
            try
            {

                var allPostActiveWithOffer = await _postSearchService.GetAllPostActiveWithOfferService();
                return Ok(allPostActiveWithOffer);

            }
            catch (Exception ex)
            {
                Console.WriteLine($"Error getting all post: {ex.Message} {ex.StackTrace}");
                return StatusCode(500, "server error");
            }
        }
        [HttpGet("getallActivebyofferid/{offerId}")]
        [Authorize(Roles = "admin, user")]
        public async Task<IActionResult> GetAllPostActiveByOfferId(int offerId)
        {
            try
            {

                var allPostActiveWithOffer = await _postSearchService.GetAllPostActiveByOfferId(offerId);
                return Ok(allPostActiveWithOffer);

            }
            catch (Exception ex)
            {
                Console.WriteLine($"Error getting all post: {ex.Message} {ex.StackTrace}");
                return StatusCode(500, "server error");
            }
        }

        //***************************************************  enpoints para admin  **************************************//
        [HttpGet("getallwithoffer")]
        [Authorize(Roles = "admin")]
        public async Task<IActionResult> GetAllPostWithOffer()
        {
            try
            {

                var allPostActiveWithOffer = await _postSearchService.GetAllPostWithOfferService();
                return Ok(allPostActiveWithOffer);

            }
            catch (Exception ex)
            {
                Console.WriteLine($"Error getting all post: {ex.Message} {ex.StackTrace}");
                return StatusCode(500, "server error");
            }
        }
    }
}
