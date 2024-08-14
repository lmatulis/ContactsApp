using ContactsApp.Client.Models;
using ContactsApp.Client.Services.Interfaces;
using ContactsApp.Components.Account;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;

namespace ContactsApp.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    [Authorize]
    public class CategoriesController : ControllerBase
    {
        private readonly ICategoryService _categoryService;

        private string _userId => User.GetUserId()!;

        public CategoriesController(ICategoryService categoryService)
        {
            _categoryService = categoryService;
        }


        [HttpGet("{id:int}")]
        public async Task<ActionResult<CategoryDTO>> GetCategoryById([FromRoute] int id)
        {
            try
            {
                CategoryDTO? category = await _categoryService.GetCategoryByIdAsync(id, _userId);
                return category == null ? NotFound() : Ok(category);
            }
            catch (Exception ex)
            {
                Console.WriteLine(ex);
                return Problem();
            }
        }



        [HttpPost]
        public async Task<ActionResult<CategoryDTO>> CreateCategory([FromBody] CategoryDTO categoryDTO)
        {
            try
            {
                CategoryDTO createdCategory = await _categoryService.CreateCategoryAsync(categoryDTO, _userId);
                return CreatedAtAction(nameof(GetCategoryById), new { id = createdCategory.Id }, createdCategory);
            }
            catch (Exception ex)
            {
                Console.WriteLine(ex);
                return Problem();
            }
        }


        [HttpGet]
        public async Task<ActionResult<IEnumerable<CategoryDTO>>> GetCategories()
        {
            try
            {
                IEnumerable<CategoryDTO> categories = [];
                categories = await _categoryService.GetCategoriesAsync(_userId);
                return Ok(categories);
            }
            catch (Exception ex)
            {
                Console.WriteLine(ex);
                return Problem();
            }
        }



        [HttpPut("{id:int}")]
        public async Task<ActionResult> UpdateCategory([FromRoute] int id, [FromBody] CategoryDTO categoryDTO)
        {
            if (id != categoryDTO.Id) { return BadRequest(); }

            try
            {
                await _categoryService.UpdateCategoryAsync(categoryDTO, _userId);
                return Ok();
            }
            catch (Exception ex)
            {
                Console.WriteLine(ex);
                return Problem();
            }
        }



        [HttpDelete("{id:int}")]
        public async Task<ActionResult> DeleteCategory([FromRoute] int id)
        {
            try
            {
                await _categoryService.DeleteCategoryAsync(id, _userId);
                return NoContent();
            }
            catch (Exception ex)
            {
                Console.WriteLine(ex);
                return Problem();
            }
        }

        [HttpPost("{id:int}/email")]
        public async Task<ActionResult> EmailCategory([FromRoute] int id, [FromBody] EmailData emailData)
        {
            try
            {
                bool success = await _categoryService.EmailCategoryAsync(id, emailData, _userId);
                return success ? Ok() : BadRequest();
            }
            catch (Exception ex)
            {
                Console.WriteLine(ex);
                return Problem();
            }
        }
    }
}
