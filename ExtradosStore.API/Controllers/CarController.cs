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

            var userIdClaim = User.Claims.FirstOrDefault(c => c.Type == ClaimTypes.NameIdentifier);

            if (userIdClaim == null) return StatusCode(401, "Unauthorized");


            int.TryParse(userIdClaim.Value, out int userId);
            var rowsAffected = await _carService.AddTocar(addToCarRequest, userId);
            return Ok("added to car");


        }
        [Authorize(Roles = "admin,user")]
        [HttpDelete("delete/{postId}")]

        public async Task<IActionResult> RemoveOneQuantityOrDeleteItemCar(int postId)
        {
            var userIdClaim = User.Claims.FirstOrDefault(c => c.Type == ClaimTypes.NameIdentifier);
            if (userIdClaim == null) return StatusCode(401, "Unauthorized");
            int.TryParse(userIdClaim.Value, out int userIdFromToken);
            var rowsAffected = await _carService.RemoveOneQuantityOrDeleteItemCar(postId, userIdFromToken);
            return Ok("succes");

        }



        [Authorize(Roles = "admin,user")]
        [HttpPost("buycar")]

        public async Task<IActionResult> BuyCar()
        {

            var userIdClaim = User.Claims.FirstOrDefault(c => c.Type == ClaimTypes.NameIdentifier);

            if (userIdClaim == null) return StatusCode(401, "Unauthorized");


            int.TryParse(userIdClaim.Value, out int userIdFromToken);

            var rowsAffected = await _carService.BuyCar(userIdFromToken);
            return Ok("succes");

        }
        [Authorize(Roles = "admin,user")]
        [HttpGet("getcar")]

        public async Task<IActionResult> GetCarByUserId()
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
        //******************************* funciones de admin ********************************************//
        [Authorize(Roles = "admin")]
        [HttpGet("getcar/{userId}")]

        public async Task<IActionResult> GetCarByUserIdAdmin(int userId)
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


    }
}

