using Microsoft.AspNetCore.Mvc;
using ProductStore.Api.DTOs;
using ProductStore.Api.Services;

namespace ProductStore.Api.Controllers;

[ApiController]
[Route("api/[controller]")]
public class ProductsController : ControllerBase
{
    private readonly IProductService _service;

    public ProductsController(IProductService service)
    {
        _service = service;
    }

    [HttpGet]
    public async Task<IActionResult> GetAll(
        [FromQuery] int page = 1,
        [FromQuery] int pageSize = 10)
    {
        var products = await _service.GetAllAsync(page, pageSize);
        return Ok(products);
    }

    [HttpGet("{id}")]
    public async Task<IActionResult> GetById(int id)
    {
        var product = await _service.GetByIdAsync(id);

        if (product == null)
            return NotFound();

        return Ok(product);
    }

    [HttpPost]
    public async Task<IActionResult> Create(ProductCreateDto dto)
    {
        var createdProduct = await _service.CreateAsync(dto);

        return CreatedAtAction(
            nameof(GetById),
            new { id = createdProduct.Id },
            createdProduct
        );
    }

    [HttpPut("{id}")]
    public async Task<IActionResult> Update(int id, ProductUpdateDto dto)
    {
        var updated = await _service.UpdateAsync(id, dto);

        if (!updated)
            return NotFound();

        return NoContent();
    }

    [HttpDelete("{id}")]
    public async Task<IActionResult> Delete(int id)
    {
        var deleted = await _service.DeleteAsync(id);

        if (!deleted)
            return NotFound();

        return NoContent();
    }
}