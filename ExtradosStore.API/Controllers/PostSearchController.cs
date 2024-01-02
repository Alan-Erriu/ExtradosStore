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
                return StatusCode(500, "Something went wrong. Please contact support.");
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
                return StatusCode(500, "Something went wrong. Please contact support.");
            }
        }

    }
}
