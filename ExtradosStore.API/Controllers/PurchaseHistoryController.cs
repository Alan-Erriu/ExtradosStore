using ExtradosStore.Services.Interfaces;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using System.Security.Claims;

namespace ExtradosStore.API.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class PurchaseHistoryController : ControllerBase
    {
        private readonly ISalesHistoryService _salesHistoryService;
        public PurchaseHistoryController(ISalesHistoryService salesHistoryService)
        {
            _salesHistoryService = salesHistoryService;
        }

        [Authorize(Roles = "admin,user")]
        [HttpGet("gethistory")]

        public async Task<IActionResult> GetPurchaseHistory()
        {

            var userIdClaim = User.Claims.FirstOrDefault(c => c.Type == ClaimTypes.NameIdentifier);

            if (userIdClaim == null) return StatusCode(401, "Unauthorized");


            int.TryParse(userIdClaim.Value, out int userIdFromToken);

            var historyUser = await _salesHistoryService.GetHistoryGetUserPurchaseHistory(userIdFromToken);

            return Ok(historyUser);
        }



        //*********************************** endpoints para admin role ******************************************************
        [Authorize(Roles = "admin")]
        [HttpGet("gethistory/{userId}")]

        public async Task<IActionResult> GetPurchaseHistoryByUserIdAdmin(int userId)
        {


            var historyUser = await _salesHistoryService.GetHistoryGetUserPurchaseHistory(userId);

            return Ok(historyUser);

        }

    }
}
