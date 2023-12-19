﻿using ExtradosStore.Services.Interfaces;
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

        public async Task<IActionResult> GetCarByUserId(int userId)
        {
            try
            {
                var userIdClaim = User.Claims.FirstOrDefault(c => c.Type == ClaimTypes.NameIdentifier);

                if (userIdClaim == null) return StatusCode(401, "Unauthorized");


                int.TryParse(userIdClaim.Value, out int userIdFromToken);

                var historyUser = await _salesHistoryService.GetHistoryGetUserPurchaseHistory(userIdFromToken);

                return Ok(historyUser);
            }
            catch (Exception Ex)
            {
                Console.WriteLine($"Error getting history user  {Ex.Message} {Ex.StackTrace}");

                return StatusCode(500, "server error:");
            }
        }

    }
}
