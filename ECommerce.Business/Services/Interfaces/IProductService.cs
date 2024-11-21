using ECommerce.DTOs;

namespace ECommerce.Business.Services.Interfaces
{
    public interface IProductService
    {
        Task<List<ProductDto>> GetAllAsync();
        Task<ProductDto> GetByIdAsync(int id);
        Task AddAsync (CreateProductDto createProductDto);
        Task UpdateAsync (int id, UpdateProductDto updateProductDto);
        Task DeleteAsync (int id);
    }
}
