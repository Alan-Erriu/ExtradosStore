using ExtradosStore.Common.CustomExceptions.CarExceptions;
using ExtradosStore.Common.CustomExceptions.PostStatusExceptions;
using ExtradosStore.Common.CustomRequest.CarRequest;
using ExtradosStore.Services.Interfaces;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using System.Security.Claims;

namespace ExtradosStore.API.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class CarController : ControllerBase
    {
        ICarService _carService;
        public CarController(ICarService carService)
        {
            _carService = carService;
        }


        [Authorize(Roles = "admin,user")]
        [HttpPost("addtocar")]

        public async Task<IActionResult> AddToCar([FromBody] AddToCarRequest addToCarRequest)
        {
            try
            {
                var userIdClaim = User.Claims.FirstOrDefault(c => c.Type == ClaimTypes.NameIdentifier);

                if (userIdClaim == null) return StatusCode(401, "Unauthorized");


                int.TryParse(userIdClaim.Value, out int userId);
                var rowsAffected = await _carService.AddTocar(addToCarRequest, userId);
                return Ok("added to car");
            }
            catch (FileNotFoundException ex)
            {
                Console.WriteLine(ex.Message);
                return NotFound("post not found");
            }
            catch (PostStatusNotFoundException ex)
            {
                Console.WriteLine(ex.Message);
                return NotFound("post Status not found");
            }
            catch (StatusIsNotActiveException ex)
            {
                Console.WriteLine(ex.Message);
                return Conflict("the status post is not active");
            }
            catch (StockIsLessThanQuantity ex)
            {
                Console.WriteLine(ex.Message);
                return Conflict("stock is less than quantity");
            }
            catch (InvalidOperationException ex)
            {
                Console.WriteLine(ex.Message);
                return BadRequest("a user cannot buy your posts");
            }
            catch (Exception Ex)
            {
                Console.WriteLine($"Error adding new post to car  {Ex.Message} {Ex.StackTrace}");

                return StatusCode(500, "server error:");
            }

        }
    }
}
