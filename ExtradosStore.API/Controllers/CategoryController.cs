using ExtradosStore.Common.CustomRequest.CategoryRequest;
using ExtradosStore.Services.Interfaces;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace ExtradosStore.API.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class CategoryController : ControllerBase
    {
        private readonly ICategoryService _categoryService;
        public CategoryController(ICategoryService categoryService)
        {
            _categoryService = categoryService;
        }



        [Authorize(Roles = "admin")]
        [HttpPost("create")]

        public async Task<IActionResult> CreateNewCategory([FromBody] CreateCategoryRequest createCategoryRequest)
        {
            var rowsAffected = await _categoryService.CreateCategoryService(createCategoryRequest.category_name);
            return Ok("category created");
        }


        [Authorize(Roles = "admin")]
        [HttpDelete("delete/{categoryId}")]

        public async Task<IActionResult> DeleteCategoryById(int categoryId)
        {
            var rowsAffected = await _categoryService.DeleteCategoryByIdService(categoryId);
            return Ok("brand deleted");
        }
        [Authorize(Roles = "admin")]
        [HttpGet("getcategorys")]

        public async Task<IActionResult> GetAllCategorys()
        {
            var listOfBrands = await _categoryService.GetAllCategorysService();
            return Ok(listOfBrands);

        }
    }
}
