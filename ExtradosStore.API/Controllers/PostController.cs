using ExtradosStore.Common.CustomRequest.PostRequest;
using ExtradosStore.Services.Interfaces;
using Microsoft.AspNetCore.Mvc;

namespace ExtradosStore.API.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class PostController : ControllerBase
    {
        private readonly IPostService _postService;
        public PostController(IPostService postService)
        {
            _postService = postService;

        }
        // registrarse como usuario - rol=(user)

        [HttpPost("create")]

        public async Task<IActionResult> CreateNewPost([FromBody] CreateNewPostRequest createNewPostRequest)
        {

            try
            {

                var rowsAffected = await _postService.CreatePostService(createNewPostRequest);
                if (rowsAffected == 0) return StatusCode(500, "server error");
                return Ok("post created");
            }
            catch (Exception Ex)
            {
                Console.WriteLine($"Error register a new user {Ex.Message} {Ex.StackTrace}");

                return StatusCode(500, "server error:");
            }

        }
    }
}
