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

            //**todo** evitar dos post con el mismo nombre (para el mismo usuario)
            var userIdClaim = User.Claims.FirstOrDefault(c => c.Type == ClaimTypes.NameIdentifier);

            if (userIdClaim == null) return StatusCode(401, "Unauthorized");

            int.TryParse(userIdClaim.Value, out int userId);

            createNewPostRequest.post_userId = userId;

            var rowsAffected = await _postService.CreatePostService(createNewPostRequest);
            if (rowsAffected == 0) return StatusCode(500, "server error");
            return Ok("post created");

        }
        [HttpPut("statuspaused/{postId}")]
        [Authorize(Roles = "admin, user")]

        public async Task<IActionResult> SetStatusToPaused(int postId)
        {

            var userIdClaim = User.Claims.FirstOrDefault(c => c.Type == ClaimTypes.NameIdentifier);
            var userRolName = User.Claims.FirstOrDefault(c => c.Type == ClaimTypes.Role);

            if (userIdClaim == null) return StatusCode(401, "Unauthorized");
            int.TryParse(userIdClaim.Value, out int userId);

            string newStatus = "paused";
            var rowsAffected = await _postService.SetPostStatus(postId, userId, userRolName.Value, newStatus);
            if (rowsAffected == 0) return StatusCode(500, "server error");
            return Ok("post status now is paused");

        }
        [HttpPut("statuscancelled/{postId}")]
        [Authorize(Roles = "admin, user")]

        public async Task<IActionResult> SetStatusToCancelled(int postId)
        {


            var userIdClaim = User.Claims.FirstOrDefault(c => c.Type == ClaimTypes.NameIdentifier);
            var userRolName = User.Claims.FirstOrDefault(c => c.Type == ClaimTypes.Role);

            if (userIdClaim == null) return StatusCode(401, "Unauthorized");
            int.TryParse(userIdClaim.Value, out int userId);

            string newStatus = "cancelled";
            var rowsAffected = await _postService.SetPostStatus(postId, userId, userRolName.Value, newStatus);
            if (rowsAffected == 0) return StatusCode(500, "server error");
            return Ok("post status now is cancelled");



        }
        [HttpPut("statusactive/{postId}/{stock}")]
        [Authorize(Roles = "admin, user")]

        public async Task<IActionResult> SetStatusToActiveAndUpdateStock(int postId, int stock)
        {

            var userIdClaim = User.Claims.FirstOrDefault(c => c.Type == ClaimTypes.NameIdentifier);
            var userRolName = User.Claims.FirstOrDefault(c => c.Type == ClaimTypes.Role);

            if (userIdClaim == null) return StatusCode(401, "Unauthorized");
            int.TryParse(userIdClaim.Value, out int userId);

            string newStatus = "active";
            var rowsAffected = await _postService.SetStatusActiveAndUpdateStock(postId, userId, newStatus, stock);
            if (rowsAffected == 0) return StatusCode(500, "server error");
            return Ok("post status now is active");

        }

        [HttpPut("update")]
        [Authorize(Roles = "admin, user")]

        public async Task<IActionResult> UpdatePost([FromBody] UpdatePostRequest updatePostRequest)
        {

            if (updatePostRequest.postPrice < 0) return BadRequest("Post price must have a valid positive value");

            if (updatePostRequest.postStock < 0) return BadRequest("Post stock must have a valid positive value");

            var userIdClaim = User.Claims.FirstOrDefault(c => c.Type == ClaimTypes.NameIdentifier);
            int.TryParse(userIdClaim.Value, out int userId);
            if (userId == 0) return StatusCode(401, "Unauthorized");
            var rowsAffected = await _postService.UpdatePostService(updatePostRequest, userId);
            if (rowsAffected == 0) return StatusCode(500, "server error");
            return Ok("update post");

        }


    }
}
