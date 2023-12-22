using ExtradosStore.Common.CustomExceptions.PostExceptions;
using ExtradosStore.Common.CustomRequest.BrandRequest;
using ExtradosStore.Services.Interfaces;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using System.Data.SqlClient;

namespace ExtradosStore.API.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class BrandController : ControllerBase
    {
        private readonly IBrandService _brandService;
        public BrandController(IBrandService brandService)
        {
            _brandService = brandService;
        }



        [Authorize(Roles = "admin")]
        [HttpPost("create")]

        public async Task<IActionResult> CreateNewBrand([FromBody] CreateBrandRequest createBrandRequest)

        {
            try
            {
                var rowsAffected = await _brandService.CreateBrandService(createBrandRequest.brand_name);
                return Ok("brand created");
            }
            catch (DuplicateNameBrandException ex)
            {
                Console.WriteLine(ex.Message);
                return Conflict("the name brand is already in use");
            }
            catch (Exception Ex)
            {
                Console.WriteLine($"Error creating a new brand:  {Ex.Message} {Ex.StackTrace}");

                return StatusCode(500, "Something went wrong. Please contact support.");
            }

        }


        [Authorize(Roles = "admin")]
        [HttpDelete("delete/{brandId}")]

        public async Task<IActionResult> DeleteBrandById(int brandId)
        {

            try
            {
                var rowsAffected = await _brandService.DeleteBrandByIdService(brandId);
                return Ok("brand deleted");
            }
            catch (KeyNotFoundException ex)
            {
                Console.WriteLine(ex.Message);
                return NotFound("id brand not found");
            }
            catch (SqlException sqlEx)
            {
                Console.WriteLine($"SQL Exception: {sqlEx.Message} {sqlEx.StackTrace}");

                if (sqlEx.Number == 547)
                {
                    return BadRequest("You cannot delete a brand associated with an existing product.");
                }
                else
                {
                    return StatusCode(500, "Something went wrong. Please contact support.");
                }
            }
            catch (Exception Ex)
            {
                Console.WriteLine($"Error deleting  brand:  {Ex.Message} {Ex.StackTrace}");

                return StatusCode(500, "Something went wrong. Please contact support.");
            }

        }
        [Authorize(Roles = "admin")]
        [HttpGet("getbrands")]

        public async Task<IActionResult> GetAllBrands()
        {

            try
            {
                var listOfBrands = await _brandService.GetAllBrandsService();
                return Ok(listOfBrands);
            }

            catch (Exception Ex)
            {
                Console.WriteLine($"Error deleting  brand:  {Ex.Message} {Ex.StackTrace}");

                return StatusCode(500, "Something went wrong. Please contact support.");
            }

        }
    }
}

