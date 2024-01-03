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


        // filtro de busquedad por categoría, marca, y  nombre. Toma en cuenta ofertas y aplica descuentos
        [HttpGet("searchpost")]
        [Authorize(Roles = "admin,user")]
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
        //todas las publicaciones con o sin oferta, con estado "active", si tiene oferta aplica el descuento.
        [HttpGet("getall")]
        [Authorize(Roles = "admin,user")]
        public async Task<IActionResult> GetAllPostActive()
        {
            try
            {

                var allPostActiveWithOffer = await _postSearchService.GetAllPostService();
                return Ok(allPostActiveWithOffer);

            }
            catch (Exception ex)
            {
                Console.WriteLine($"Error getting all post: {ex.Message} {ex.StackTrace}");
                return StatusCode(500, "Something went wrong. Please contact support.");
            }
        }

        //todas las publicaciones con oferta, con estado "active"
        [HttpGet("getallwithoffer")]
        [Authorize(Roles = "admin,user")]
        public async Task<IActionResult> GetAllPostActiveWithOffer()
        {
            try
            {

                var allPostActiveWithOffer = await _postSearchService.GetAllPostWithOffer();
                return Ok(allPostActiveWithOffer);

            }
            catch (Exception ex)
            {
                Console.WriteLine($"Error getting all post: {ex.Message} {ex.StackTrace}");
                return StatusCode(500, "Something went wrong. Please contact support.");
            }
        }



        //buscar una publicacion por id, aplica ofertas (si las tiene), solo busca publicaciones activas
        [HttpGet("getpost/{postId}")]
        [Authorize(Roles = "admin,user")]
        public async Task<IActionResult> GetPostByPostId(int postId)
        {
            try
            {

                var post = await _postSearchService.GetPostByPostId(postId);
                return Ok(post);

            }
            catch (Exception ex)
            {
                Console.WriteLine($"Error getting all post: {ex.Message} {ex.StackTrace}");
                return StatusCode(500, "Something went wrong. Please contact support.");
            }
        }

        //obtener todas las publicaciones de un usuario
        [HttpGet("getallpostbyuser/{userId}")]
        [Authorize(Roles = "admin")]
        public async Task<IActionResult> GetAllPostByUserId(int userId)
        {
            try
            {

                var post = await _postSearchService.GetAllPostByUserId(userId);
                return Ok(post);

            }
            catch (Exception ex)
            {
                Console.WriteLine($"Error getting all post: {ex.Message} {ex.StackTrace}");
                return StatusCode(500, "Something went wrong. Please contact support.");
            }
        }
    }
}
