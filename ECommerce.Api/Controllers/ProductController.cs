﻿using ECommerce.Business.Services.Interfaces;
using ECommerce.DTOs;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace ECommerce.Api.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class ProductController : ControllerBase
    {
        private readonly IProductService _productService;

        public ProductController(IProductService productService)
        {
            _productService = productService;
        }

        [Authorize]
        [HttpGet]
        public async Task<IActionResult> GetAll([FromQuery] PaginationDto paginationDto, [FromQuery] string? sortBy = "name", [FromQuery] string? order = "asc", [FromQuery] int? categoryId = null, [FromQuery] string? search = null)
        {
            var products = await _productService.GetAllAsync(paginationDto.Page, paginationDto.PageSize, sortBy, order, categoryId, search);
            return Ok(products);
        }

        [Authorize]
        [HttpGet("{id}")]
        public async Task<IActionResult> GetById(int id)
        {
            try
            {
                var product = await _productService.GetByIdAsync(id);
                return Ok(product);
            }
            catch (Exception ex)
            {
                return NotFound(new { message = ex.Message});
            }
        }

        [Authorize(Roles = "Admin")]
        [HttpPost]
        public async Task<IActionResult> Add([FromBody] CreateProductDto createProductDto)
        {
            await _productService.AddAsync(createProductDto);
            return Created("", new { message = "Product successfully created"});
        }

        [Authorize(Roles = "Admin")]
        [HttpPut("{id}")]
        public async Task<IActionResult> Update(int id, [FromBody] UpdateProductDto updateProductDto)
        {
            
            try
            {
                await _productService.UpdateAsync(id, updateProductDto);
                return Ok(new { message = "Product successfully updated"});
            }
            catch (Exception ex)
            {
                return NotFound(new { message = ex.Message});
            }
        }

        [Authorize(Roles = "Admin")]
        [HttpDelete("{id}")]
        public async Task<IActionResult> Delete(int id)
        {
            try
            {
                await _productService.DeleteAsync(id);
                return Ok(new { message = "Product succesfully deleted"});
            }
            catch (Exception ex)
            {
                return NotFound(new { message = ex.Message});
            }
        }
    }
}
