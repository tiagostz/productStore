using ProductStore.Api.DTOs;

namespace ProductStore.Api.Services;

public interface IProductService
{
    Task<object> GetAllAsync(int page, int pageSize);

    Task<ProductResponseDto?> GetByIdAsync(int id);

    Task<ProductResponseDto> CreateAsync(ProductCreateDto dto);

    Task<bool> UpdateAsync(int id, ProductUpdateDto dto);

    Task<bool> DeleteAsync(int id);
}