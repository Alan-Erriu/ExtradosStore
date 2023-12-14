using ExtradosStore.Common.CustomRequest.PostRequest;
using ExtradosStore.Services.Interfaces;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using System.Security.Claims;

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
        [Authorize(Roles = "admin, user")]
        public async Task<IActionResult> CreateNewPost([FromBody] CreateNewPostRequest createNewPostRequest)
        {
            try
            {

                var userIdClaim = User.Claims.FirstOrDefault(c => c.Type == ClaimTypes.NameIdentifier);

                if (userIdClaim == null) return StatusCode(401, "Unauthorized");


                if (int.TryParse(userIdClaim.Value, out int userId))
                {
                    createNewPostRequest.post_userId = userId;

                    var rowsAffected = await _postService.CreatePostService(createNewPostRequest);
                    if (rowsAffected == 0) return StatusCode(500, "server error");


                    return Ok("post created");
                }
                else
                {
                    return StatusCode(500, "Error converting user ID to integer");
                }
            }
            catch (Exception ex)
            {
                Console.WriteLine($"Error creating a new post: {ex.Message}\n{ex.StackTrace}");
                return StatusCode(500, "server error");
            }
        }
        [HttpGet("getall")]
        [Authorize(Roles = "admin, user")]
        public async Task<IActionResult> GetAllPostActiveWithOffer()
        {
            try
            {

                var allPostActiveWithOffer = await _postService.GetAllPostActiveWithOffer();
                return Ok(allPostActiveWithOffer);

            }
            catch (Exception ex)
            {
                Console.WriteLine($"Error getting all post: {ex.Message} {ex.StackTrace}");
                return StatusCode(500, "server error");
            }
        }
        [HttpGet("getall2")]
        //[Authorize(Roles = "admin, user")]
        public async Task<IActionResult> GetAllPostActiveWithOffer2()
        {
            try
            {

                var allPostActiveWithOffer = await _postService.GetAllPostActiveService2();
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
