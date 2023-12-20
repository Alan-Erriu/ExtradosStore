using ExtradosStore.Common.CustomRequest.PostSearchRequest;
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


        // todos las publicaciones activas, sin ofertas.
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


        // todas las publicaciones activas con ofertas tambien activas.
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


        // todas las publicaciones activas ligadas a una oferta activa
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

        // filtro de busquedad por categoría, marca, y por nombre. No toma en cuenta ofertas.
        [HttpGet("searchpost")]
        [Authorize(Roles = "admin")]
        public async Task<IActionResult> SearhPost(PostSearchRequest postSearchRequest)
        {
            try
            {
                var allPost = await _postSearchService.SearchPost(postSearchRequest);
                return Ok(allPost);
            }
            catch (Exception ex)
            {
                Console.WriteLine($"Error getting all post: {ex.Message} {ex.StackTrace}");
                return StatusCode(500, "server error");
            }
        }
        //todas las publicaciones con ofertas vencidas y no vencidas
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
        //todas las publicaciones de un usuario, con y sin oferta.
        [HttpGet("getallbyuser/{userId}")]
        [Authorize(Roles = "admin")]
        public async Task<IActionResult> GetAllPostByUserId(int userId)
        {
            try
            {

                var allPost = await _postSearchService.GetAllPostByUserId(userId);
                return Ok(allPost);

            }
            catch (Exception ex)
            {
                Console.WriteLine($"Error getting all post: {ex.Message} {ex.StackTrace}");
                return StatusCode(500, "server error");
            }
        }
    }
}
