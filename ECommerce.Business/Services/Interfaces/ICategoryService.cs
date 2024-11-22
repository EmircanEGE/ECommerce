using ECommerce.DTOs;

namespace ECommerce.Business.Services.Interfaces
{
    public interface ICategoryService
    {
        Task<List<CategoryDto>> GetAllAsync(int page, int pageSize, string sortBy, string order, string? search);
        Task<CategoryDto> GetByIdAsync(int id);
        Task AddAsync(CreateCategoryDto createCategoryDto);
        Task UpdateAsync(int id, UpdateCategoryDto updateCategoryDto);
        Task DeleteAsync(int id);
    }
}
