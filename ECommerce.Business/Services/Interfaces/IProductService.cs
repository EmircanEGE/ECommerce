using ECommerce.DTOs;

namespace ECommerce.Business.Services.Interfaces
{
    public interface IProductService
    {
        Task<List<ProductDto>> GetAllAsync(int page, int pageSize, string sortBy, string order, int? categoryId, string? search);
        Task<ProductDto> GetByIdAsync(int id);
        Task AddAsync (CreateProductDto createProductDto);
        Task UpdateAsync (int id, UpdateProductDto updateProductDto);
        Task DeleteAsync (int id);
    }
}
