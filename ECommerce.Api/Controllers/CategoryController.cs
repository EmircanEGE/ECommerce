using ECommerce.Business.Services.Interfaces;
using ECommerce.DTOs;
using Microsoft.AspNetCore.Mvc;

namespace ECommerce.Api.Controllers
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

        [HttpGet]
        public async Task<IActionResult> GetAll([FromQuery] PaginationDto paginationDto, [FromQuery] string? sortBy = "name", [FromQuery] string? order = "asc")
        {
            var categories = await _categoryService.GetAllAsync(paginationDto.Page, paginationDto.PageSize, sortBy, order);
            return Ok(categories);
        }

        [HttpGet("{id}")]
        public async Task<IActionResult> GetById(int id)
        {
            try
            {
                var category = await _categoryService.GetByIdAsync(id);
                return Ok(category);
            }
            catch (Exception ex) 
            { 
                return NotFound(new { message = ex.Message});
            }
        }

        [HttpPost]
        public async Task<IActionResult> Add([FromBody] CreateCategoryDto createCategoryDto)
        {
            await _categoryService.AddAsync(createCategoryDto);
            return Created("", new {message = "Category created successfully"});
        }

        [HttpPut("{id}")]
        public async Task<IActionResult> Update(int id, [FromBody] UpdateCategoryDto updateCategoryDto)
        {
            try
            {
                await _categoryService.UpdateAsync(id, updateCategoryDto);
                return Ok(new { message = "Category updated successfully" });
            }
            catch (Exception ex)
            {
                return NotFound(new { message = ex.Message});
            }
        }

        [HttpDelete("{id}")]
        public async Task<IActionResult> Delete(int id)
        {
            try
            {
                await _categoryService.DeleteAsync(id);
                return Ok(new { message = "Category deleted  successfully"});
            }
            catch(Exception ex)
            {
                return NotFound(new { message = ex.Message});
            }

        }
    }
}
