using Microsoft.EntityFrameworkCore;
using ProductStore.Api.Data;
using ProductStore.Api.DTOs;
using ProductStore.Api.Entities;

namespace ProductStore.Api.Services;

public class ProductService : IProductService
{
    private readonly AppDbContext _context;
    private readonly ILogger<ProductService> _logger;

    public ProductService(
        AppDbContext context,
        ILogger<ProductService> logger)
    {
        _context = context;
        _logger = logger;
    }

    public async Task<object> GetAllAsync(int page, int pageSize)
{
    var totalItems = await _context.Products.CountAsync();

    var products = await _context.Products
        .Skip((page - 1) * pageSize)
        .Take(pageSize)
        .ToListAsync();

    return new
    {
        items = products.Select(MapToResponseDto),
        totalItems,
        totalPages = (int)Math.Ceiling(totalItems / (double)pageSize),
        currentPage = page,
        pageSize
    };
}

    public async Task<ProductResponseDto?> GetByIdAsync(int id)
    {
        var product = await _context.Products.FindAsync(id);

        if (product == null)
            return null;

        return MapToResponseDto(product);
    }

    public async Task<ProductResponseDto> CreateAsync(ProductCreateDto dto)
    {
        // Regra 1
        if (dto.StockQuantity < 0)
        {
            _logger.LogWarning("Validation error: O estoque não pode ser negativo.");
           throw new Exception("O estoque não pode ser negativo.");
        }

        // Regra 2
        if (dto.Category.ToLower() == "eletronicos" && dto.Price < 50)
        {
            _logger.LogWarning("Validation error: preço mínimo inválido para Eletronicos.");
            throw new Exception("Produtos da categoria Eletronicos devem possuir preço mínimo de 50.");
        }

        // Regra 3
        var skuExists = await _context.Products
            .AnyAsync(p => p.SKU == dto.SKU);

        if (skuExists)
        {
            _logger.LogWarning("Validation error: O SKU informado já está cadastrado.");
            throw new Exception("O SKU informado já está cadastrado.");
        }

        var product = new Product
        {
            Name = dto.Name,
            SKU = dto.SKU,
            Category = dto.Category,
            Price = dto.Price,
            StockQuantity = dto.StockQuantity
        };

        _context.Products.Add(product);

        await _context.SaveChangesAsync();

        _logger.LogInformation(
            "Produto criado com sucesso. Id: {Id}, SKU: {SKU}",
            product.Id,
            product.SKU
        );

        return MapToResponseDto(product);
    }

    public async Task<bool> UpdateAsync(int id, ProductUpdateDto dto)
    {
        var product = await _context.Products.FindAsync(id);

        if (product == null)
            return false;

        if (dto.StockQuantity < 0)
        {
            _logger.LogWarning("Validation error: O estoque não pode ser negativo.");
            throw new Exception("O estoque não pode ser negativo.");
        }

        if (dto.Category.ToLower() == "eletronicos" && dto.Price < 50)
        {
            _logger.LogWarning("Validation error: preço mínimo inválido para Eletronicos.");
            throw new Exception("Produtos da categoria Eletronicos devem possuir preço mínimo de 50.");
        }

        product.Name = dto.Name;
        product.Category = dto.Category;
        product.Price = dto.Price;
        product.StockQuantity = dto.StockQuantity;

        await _context.SaveChangesAsync();

        _logger.LogInformation(
            "Produto atualizado com sucesso. Id: {Id}",
            product.Id
        );

        return true;
    }

    public async Task<bool> DeleteAsync(int id)
    {
        var product = await _context.Products.FindAsync(id);

        if (product == null)
            return false;

        _context.Products.Remove(product);

        await _context.SaveChangesAsync();

        _logger.LogInformation(
            "Produto removido com sucesso. Id: {Id}",
            product.Id
        );

        return true;
    }

    private static ProductResponseDto MapToResponseDto(Product product)
    {
        return new ProductResponseDto
        {
            Id = product.Id,
            Name = product.Name,
            SKU = product.SKU,
            Category = product.Category,
            Price = product.Price,
            StockQuantity = product.StockQuantity,
            CreatedAt = product.CreatedAt
        };
    }
}