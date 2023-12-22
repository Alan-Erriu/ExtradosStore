using ExtradosStore.Common.CustomExceptions.PostExceptions;
using ExtradosStore.Common.CustomRequest.CategoryRequest;
using ExtradosStore.Services.Interfaces;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using System.Data.SqlClient;

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

            try
            {
                var rowsAffected = await _categoryService.CreateCategoryService(createCategoryRequest.category_name);
                return Ok("category created");
            }
            catch (DuplicateNameCategoryException ex)
            {
                Console.WriteLine(ex.Message);
                return Conflict("the name category is already in use");
            }
            catch (Exception Ex)
            {
                Console.WriteLine($"Error creating a new category:  {Ex.Message} {Ex.StackTrace}");

                return StatusCode(500, "Something went wrong. Please contact support.");
            }

        }


        [Authorize(Roles = "admin")]
        [HttpDelete("delete/{categoryId}")]

        public async Task<IActionResult> DeleteCategoryById(int categoryId)
        {

            try
            {
                var rowsAffected = await _categoryService.DeleteCategoryByIdService(categoryId);
                return Ok("brand deleted");
            }
            catch (KeyNotFoundException ex)
            {
                Console.WriteLine(ex.Message);
                return NotFound("id category not found");
            }
            catch (SqlException sqlEx)
            {
                Console.WriteLine($"SQL Exception: {sqlEx.Message} {sqlEx.StackTrace}");

                if (sqlEx.Number == 547)
                {
                    return BadRequest("You cannot delete a category associated with an existing post.");
                }
                else
                {
                    return StatusCode(500, "Something went wrong. Please contact support.");
                }
            }
            catch (Exception Ex)
            {
                Console.WriteLine($"Error deleting  category:  {Ex.Message} {Ex.StackTrace}");

                return StatusCode(500, "Something went wrong. Please contact support.");
            }

        }
        [Authorize(Roles = "admin")]
        [HttpGet("getcategorys")]

        public async Task<IActionResult> GetAllCategorys()
        {

            try
            {
                var listOfBrands = await _categoryService.GetAllCategorysService();
                return Ok(listOfBrands);
            }

            catch (Exception Ex)
            {
                Console.WriteLine($"Error gettings  categorys:  {Ex.Message} {Ex.StackTrace}");

                return StatusCode(500, "Something went wrong. Please contact support.");
            }

        }
    }
}
