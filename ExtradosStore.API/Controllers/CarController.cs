using ExtradosStore.Common.CustomExceptions.CarExceptions;
using ExtradosStore.Common.CustomExceptions.PostExceptions;
using ExtradosStore.Common.CustomExceptions.PostStatusExceptions;
using ExtradosStore.Common.CustomRequest.CarRequest;
using ExtradosStore.Common.CustomResponse;
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
        [Authorize(Roles = "admin,user")]
        [HttpDelete("delete/{postId}")]

        public async Task<IActionResult> RemoveOneQuantityOrDeleteItemCar(int postId)
        {
            try
            {
                var userIdClaim = User.Claims.FirstOrDefault(c => c.Type == ClaimTypes.NameIdentifier);

                if (userIdClaim == null) return StatusCode(401, "Unauthorized");


                int.TryParse(userIdClaim.Value, out int userIdFromToken);

                var rowsAffected = await _carService.RemoveOneQuantityOrDeleteItemCar(postId, userIdFromToken);


                return Ok("succes");


            }
            catch (PostNotFoundException ex)
            {
                Console.WriteLine(ex.Message);
                return NotFound("post not found in car");

            }
            catch (Exception Ex)
            {
                Console.WriteLine($"Error deleting post to car  {Ex.Message} {Ex.StackTrace}");

                return StatusCode(500, "server error:");
            }
        }
        [Authorize(Roles = "admin,user")]
        [HttpPost("buycar")]

        public async Task<IActionResult> BuyCar()
        {
            try
            {
                var userIdClaim = User.Claims.FirstOrDefault(c => c.Type == ClaimTypes.NameIdentifier);

                if (userIdClaim == null) return StatusCode(401, "Unauthorized");


                int.TryParse(userIdClaim.Value, out int userIdFromToken);

                var rowsAffected = await _carService.BuyCar(userIdFromToken);
                return Ok("succes");
            }
            catch (Exception ex)
            {

                Console.WriteLine($"Error buying car {ex.Message} {ex.StackTrace}");
                return StatusCode(500, "server error");
            }
        }
        [Authorize(Roles = "admin,user")]
        [HttpGet("getcar")]

        public async Task<IActionResult> GetCarByUserId()
        {
            try
            {
                var userIdClaim = User.Claims.FirstOrDefault(c => c.Type == ClaimTypes.NameIdentifier);

                if (userIdClaim == null) return StatusCode(401, "Unauthorized");


                int.TryParse(userIdClaim.Value, out int userIdFromToken);
                var carItems = await _carService.GetCarByUserId(userIdFromToken);


                decimal totalCost = carItems.Sum(item => item.price * item.quantity);

                CarResponse carResponse = new CarResponse
                {
                    post = carItems,
                    total = totalCost
                };

                return Ok(carResponse);
            }
            catch (Exception Ex)
            {
                Console.WriteLine($"Error adding new post to car  {Ex.Message} {Ex.StackTrace}");

                return StatusCode(500, "server error:");
            }
        }
        //******************************* funciones de admin ********************************************//
        [Authorize(Roles = "admin")]
        [HttpGet("getcar/{userId}")]

        public async Task<IActionResult> GetCarByUserIdAdmin(int userId)
        {
            try
            {



                var carItems = await _carService.GetCarByUserId(userId);


                decimal totalCost = carItems.Sum(item => item.price * item.quantity);

                CarResponse carResponse = new CarResponse
                {
                    post = carItems,
                    total = totalCost
                };

                return Ok(carResponse);
            }
            catch (Exception Ex)
            {
                Console.WriteLine($"Error adding new post to car  {Ex.Message} {Ex.StackTrace}");

                return StatusCode(500, "server error:");
            }
        }
    }
}

