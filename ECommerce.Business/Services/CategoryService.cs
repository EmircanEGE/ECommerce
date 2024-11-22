using AutoMapper;
using ECommerce.Business.Services.Interfaces;
using ECommerce.DataAccess;
using ECommerce.DTOs;
using ECommerce.Entities;
using Microsoft.AspNetCore.Mvc.RazorPages;
using Microsoft.EntityFrameworkCore;

namespace ECommerce.Business.Services
{
    public class CategoryService : ICategoryService        
    {
        private readonly ECommerceContext _context;
        private readonly IMapper _mapper;

        public CategoryService(ECommerceContext context, IMapper mapper)
        {
            _context = context;
            _mapper = mapper;
        }

        public async Task AddAsync(CreateCategoryDto createCategoryDto)
        {
            var category = _mapper.Map<Category>(createCategoryDto);
            _context.Categories.Add(category);
            await _context.SaveChangesAsync();
        }

        public async Task DeleteAsync(int id)
        {
            var category = await _context.Categories.FindAsync(id);
            if (category == null)
                throw new Exception("Category not found!");

            _context.Categories.Remove(category);
            await _context.SaveChangesAsync();
        }

        public async Task<List<CategoryDto>> GetAllAsync(int page, int pageSize, string sortBy, string order, string? search)
        {
            var query = _context.Categories.AsQueryable();

            if (!string.IsNullOrEmpty(search))
                query = query.Where(x => x.Name.ToLower().Contains(search.ToLower()));

            query = sortBy.ToLower() switch
            {
                _ => order.ToLower() == "desc" ? query.OrderByDescending(p => p.Name) : query.OrderBy(p => p.Name)
            };

            var categories = await query
                .Skip((page - 1) * pageSize)
                .Take(pageSize)
                .ToListAsync();
            return _mapper.Map<List<CategoryDto>>(categories);
        }

        public async Task<CategoryDto> GetByIdAsync(int id)
        {
            var category = await _context.Categories.FindAsync(id);
            if (category == null)
                    throw new Exception("Category not found!");
            return _mapper.Map<CategoryDto>(category);
        }

        public async Task UpdateAsync(int id, UpdateCategoryDto updateCategoryDto)
        {
            var category = await _context.Categories.FindAsync(id);
            if (category == null)
                throw new Exception("Category not found!");
            _mapper.Map(updateCategoryDto, category);
            await _context.SaveChangesAsync();
        }
    }
}
