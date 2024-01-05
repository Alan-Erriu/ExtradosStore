using ExtradosStore.Common.CustomRequest.BrandRequest;
using ExtradosStore.Services.Interfaces;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

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

            var rowsAffected = await _brandService.CreateBrandService(createBrandRequest.brand_name);
            return Ok("brand created");


        }


        [Authorize(Roles = "admin")]
        [HttpDelete("delete/{brandId}")]

        public async Task<IActionResult> DeleteBrandById(int brandId)
        {
            var rowsAffected = await _brandService.DeleteBrandByIdService(brandId);
            return Ok("brand deleted");
        }

        [Authorize(Roles = "admin")]
        [HttpGet("getbrands")]

        public async Task<IActionResult> GetAllBrands()
        {
            var listOfBrands = await _brandService.GetAllBrandsService();
            return Ok(listOfBrands);
        }
    }
}

