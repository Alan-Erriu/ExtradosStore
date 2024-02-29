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
            var allPost = await _postSearchService.SearchPost(postSearchRequest);
            return Ok(allPost);
        }
        //todas las publicaciones con o sin oferta, con estado "active", si tiene oferta aplica el descuento.
        [HttpGet("getall")]
        //[Authorize(Roles = "admin,user")]
        public async Task<IActionResult> GetAllPostActive()
        {
            var allPostActiveWithOffer = await _postSearchService.GetAllPostService();
            return Ok(allPostActiveWithOffer);
        }

        //todas las publicaciones con oferta, con estado "active"
        [HttpGet("getallwithoffer")]
        [Authorize(Roles = "admin,user")]
        public async Task<IActionResult> GetAllPostActiveWithOffer()
        {
            var allPostActiveWithOffer = await _postSearchService.GetAllPostWithOffer();
            return Ok(allPostActiveWithOffer);
        }



        //buscar una publicacion por id, aplica ofertas (si las tiene), solo busca publicaciones activas
        [HttpGet("getpost/{postId}")]
        [Authorize(Roles = "admin,user")]
        public async Task<IActionResult> GetPostByPostId(int postId)
        {
            var post = await _postSearchService.GetPostByPostId(postId);
            return Ok(post);
        }

        //obtener todas las publicaciones de un usuario
        [HttpGet("getallpostbyuser/{userId}")]
        [Authorize(Roles = "admin")]
        public async Task<IActionResult> GetAllPostByUserId(int userId)
        {
            var post = await _postSearchService.GetAllPostByUserId(userId);
            return Ok(post);
        }
    }
}
