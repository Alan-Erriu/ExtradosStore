using ExtradosStore.Common.CustomExceptions.OfferPostExceptions;
using ExtradosStore.Common.CustomRequest.OfferRequest;
using ExtradosStore.Services.Interfaces;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using System.Security.Claims;

namespace ExtradosStore.API.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class OfferPostController : ControllerBase
    {
        private readonly IOfferPostService _offerPostService;
        public OfferPostController(IOfferPostService offerPostService)
        {
            _offerPostService = offerPostService;
        }


        [HttpPost("create")]
        [Authorize(Roles = "admin, user")]

        public async Task<IActionResult> AddPostToOffer([FromBody] AddPostToOfferRequest addPostToOfferRequest)
        {
            try
            {


                var userIdClaim = User.Claims.FirstOrDefault(c => c.Type == ClaimTypes.NameIdentifier);
                var userRolName = User.Claims.FirstOrDefault(c => c.Type == ClaimTypes.Role);

                if (userIdClaim == null) return StatusCode(401, "Unauthorized");


                int.TryParse(userIdClaim.Value, out int userId);
                var rowsAffected = await _offerPostService.AddPostToOfferService(addPostToOfferRequest, userId);
                if (rowsAffected == 0) return StatusCode(500, "server error");
                return Ok("product added to offer");
            }
            catch (UnauthorizedAccessException ex)
            {
                Console.WriteLine(ex.Message);
                return Unauthorized();
            }
            catch (PostAlreadyInOfferException ex)
            {
                Console.WriteLine(ex.Message);
                return Conflict("This publication already belongs to an active offer");
            }
            catch (InvalidOperationException ex)
            {
                Console.WriteLine(ex.Message);
                return BadRequest("Only active posts can be added to offers");
            }
            catch (Exception ex)
            {

                Console.WriteLine($"error:  {ex}");
                Console.WriteLine($"stack trace: {ex.StackTrace}");
                return StatusCode(500, "Something went wrong. Please contact support.");
            }
        }
        [HttpDelete("delete/{postId}")]
        [Authorize(Roles = "admin, user")]

        public async Task<IActionResult> DeleteOfferPostByPostId(int postId)
        {
            try
            {

                var userIdClaim = User.Claims.FirstOrDefault(c => c.Type == ClaimTypes.NameIdentifier);
                var userRolName = User.Claims.FirstOrDefault(c => c.Type == ClaimTypes.Role);

                if (userIdClaim == null) return StatusCode(401, "Unauthorized");

                int.TryParse(userIdClaim.Value, out int userId);
                var rowsAffected = await _offerPostService.DeleteOfferPostByPostId(postId, userId, userRolName.Value);

                return Ok("offer post deleted");
            }
            catch (UnauthorizedAccessException ex)
            {
                Console.WriteLine(ex.Message);
                return Unauthorized();
            }
            catch (KeyNotFoundException ex)
            {
                Console.WriteLine(ex.Message);
                return NotFound("offer post not found");
            }
            catch (Exception ex)
            {

                Console.WriteLine($"error:  {ex}");
                Console.WriteLine($"stack trace: {ex.StackTrace}");
                return StatusCode(500, "Something went wrong. Please contact support.");
            }
        }
        //*************************** solo para admin ***********************
        [HttpDelete("deleteall/{offerID}")]
        [Authorize(Roles = "admin")]

        public async Task<IActionResult> DeleteAllOfferPostByOfferId(int offerID)
        {
            try
            {


                var rowsAffected = await _offerPostService.DeleteAllOfferPostByOfferId(offerID);
                if (rowsAffected == 0) return BadRequest("testeando enpoint");
                return Ok("All offer posts were deleted");
            }

            catch (Exception ex)
            {

                Console.WriteLine($"error:  {ex}");
                Console.WriteLine($"stack trace: {ex.StackTrace}");
                return StatusCode(500, "Something went wrong. Please contact support.");
            }
        }
    }
}
