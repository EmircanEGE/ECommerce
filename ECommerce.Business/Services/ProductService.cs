using AutoMapper;
using ECommerce.Business.Services.Interfaces;
using ECommerce.DataAccess;
using ECommerce.DTOs;
using ECommerce.Entities;
using Microsoft.EntityFrameworkCore;

namespace ECommerce.Business.Services
{
    public class ProductService : IProductService
    {
        private readonly ECommerceContext _context;
        private readonly IMapper _mapper;

        public ProductService(ECommerceContext context, IMapper mapper)
        {
            _context = context;
            _mapper = mapper;
        }

        public async Task AddAsync(CreateProductDto createProductDto)
        {
            var product = _mapper.Map<Product>(createProductDto);
            _context.Products.Add(product);
            await _context.SaveChangesAsync();
        }

        public async Task DeleteAsync(int id)
        {
            var product = await _context.Products.FindAsync(id);
            if (product == null)
                throw new Exception("Product not found!");

            _context.Remove(product);
            await _context.SaveChangesAsync();
        }

        public async Task<List<ProductDto>> GetAllAsync(int page, int pageSize, string sortBy, string order, int? categoryId, string? search)
        {
            var query = _context.Products.AsQueryable();

            if(categoryId.HasValue)
                query = query.Where(x => x.CategoryId == categoryId);

            if(!string.IsNullOrEmpty(search))
                query = query.Where(x => x.Name.ToLower().Contains(search.ToLower()));

            query = sortBy.ToLower() switch
            {
                "price" => order.ToLower() == "desc" ? query.OrderByDescending(p => p.Price) : query.OrderBy(p => p.Price),
                "stock" => order.ToLower() == "desc" ? query.OrderByDescending(p => p.Stock) : query.OrderBy(p => p.Stock),
                _ => order.ToLower() == "desc" ? query.OrderByDescending(p => p.Name) : query.OrderBy(p => p.Name)

            };

            var products = await query
                .Skip((page - 1) * pageSize)
                .Take(pageSize)
                .ToListAsync();
            return _mapper.Map<List<ProductDto>>(products);
        }

        public async Task<ProductDto> GetByIdAsync(int id)
        {
            var product = await _context.Products.FindAsync(id);
            if (product == null)
                throw new Exception("Product not found!");
            return _mapper.Map<ProductDto>(product);
        }

        public async Task UpdateAsync(int id, UpdateProductDto updateProductDto)
        {
            var product = await _context.Products.FindAsync(id);
            if (product == null)
                throw new Exception("Product not found!");

            _mapper.Map(updateProductDto, product);
            await _context.SaveChangesAsync();
        }
    }
}
