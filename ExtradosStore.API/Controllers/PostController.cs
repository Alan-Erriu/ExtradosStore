using ExtradosStore.Common.CustomExceptions.PostStatusExceptions;
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


        [HttpPost("create")]
        [Authorize(Roles = "admin, user")]
        public async Task<IActionResult> CreateNewPost([FromBody] CreateNewPostRequest createNewPostRequest)
        {
            try
            {

                var userIdClaim = User.Claims.FirstOrDefault(c => c.Type == ClaimTypes.NameIdentifier);

                if (userIdClaim == null) return StatusCode(401, "Unauthorized");


                int.TryParse(userIdClaim.Value, out int userId);

                createNewPostRequest.post_userId = userId;

                var rowsAffected = await _postService.CreatePostService(createNewPostRequest);
                if (rowsAffected == 0) return StatusCode(500, "server error");


                return Ok("post created");


            }
            catch (Exception ex)
            {
                Console.WriteLine($"Error creating a new post: {ex.Message}\n{ex.StackTrace}");
                return StatusCode(500, "server error");
            }
        }
        [HttpPut("statuspaused/{postId}")]
        [Authorize(Roles = "admin, user")]

        public async Task<IActionResult> SetStatusToPaused(int postId)
        {
            try
            {

                var userIdClaim = User.Claims.FirstOrDefault(c => c.Type == ClaimTypes.NameIdentifier);
                var userRolName = User.Claims.FirstOrDefault(c => c.Type == ClaimTypes.Role);

                if (userIdClaim == null) return StatusCode(401, "Unauthorized");
                int.TryParse(userIdClaim.Value, out int userId);
                // atado con alambre :(
                string newStatus = "paused";
                var rowsAffected = await _postService.SetStatusActiveToPaused(postId, userId, userRolName.Value, newStatus);
                if (rowsAffected == 0) return StatusCode(500, "server error");
                return Ok("post status now is paused");


            }
            catch (UnauthorizedAccessException ex)
            {
                Console.WriteLine(ex.Message);
                return Unauthorized();
            }
            catch (PostStatusNotFoundException ex)
            {
                Console.WriteLine(ex.Message);
                return NotFound("status id not found");
            }
            catch (Exception ex)
            {
                Console.WriteLine($"error when modifying the status of the publication: {ex.Message} {ex.StackTrace}");
                return StatusCode(500, "server error");
            }
        }
        [HttpPut("statuscancelled/{postId}")]
        [Authorize(Roles = "admin, user")]

        public async Task<IActionResult> SetStatusToCancelled(int postId)
        {
            try
            {

                var userIdClaim = User.Claims.FirstOrDefault(c => c.Type == ClaimTypes.NameIdentifier);
                var userRolName = User.Claims.FirstOrDefault(c => c.Type == ClaimTypes.Role);

                if (userIdClaim == null) return StatusCode(401, "Unauthorized");
                int.TryParse(userIdClaim.Value, out int userId);
                // atado con alambre :(
                string newStatus = "cancelled";
                var rowsAffected = await _postService.SetStatusActiveToPaused(postId, userId, userRolName.Value, newStatus);
                if (rowsAffected == 0) return StatusCode(500, "server error");
                return Ok("post status now is paused");


            }
            catch (UnauthorizedAccessException ex)
            {
                Console.WriteLine(ex.Message);
                return Unauthorized();
            }
            catch (PostStatusNotFoundException ex)
            {
                Console.WriteLine(ex.Message);
                return NotFound("status id not found");
            }
            catch (Exception ex)
            {
                Console.WriteLine($"error when modifying the status of the publication: {ex.Message} {ex.StackTrace}");
                return StatusCode(500, "server error");
            }
        }

        [HttpGet("getall")]
        [Authorize(Roles = "admin, user")]
        public async Task<IActionResult> GetAllPostActiveWithOffer()
        {
            try
            {

                var allPostActiveWithOffer = await _postService.GetAllPostActiveService();
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
