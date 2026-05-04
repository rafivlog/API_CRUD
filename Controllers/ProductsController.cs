using APICRUD.IServices;
using APICRUD.Models;
using Microsoft.AspNetCore.Mvc;

namespace APICRUD.Controllers;

[ApiController]
[Route("api/products")]
public class ProductsController : ControllerBase
{
    private readonly IProductRepository _repository;

    public ProductsController(IProductRepository repository)
    {
        _repository = repository;
    }

    /// <summary>List all products.</summary>
    [HttpGet]
    [ProducesResponseType(typeof(IReadOnlyList<Product>), StatusCodes.Status200OK)]
    public async Task<ActionResult<IReadOnlyList<Product>>> GetAll(CancellationToken cancellationToken)
    {
        var items = await _repository.GetAllAsync(cancellationToken);
        return Ok(items);
    }

    /// <summary>Get a product by id.</summary>
    [HttpGet("{id:int}")]
    [ProducesResponseType(typeof(Product), StatusCodes.Status200OK)]
    [ProducesResponseType(StatusCodes.Status404NotFound)]
    public async Task<ActionResult<Product>> GetById(int id, CancellationToken cancellationToken)
    {
        var product = await _repository.GetByIdAsync(id, cancellationToken);
        if (product is null)
            return NotFound();

        return Ok(product);
    }

    /// <summary>Create a product.</summary>
    [HttpPost]
    [ProducesResponseType(typeof(Product), StatusCodes.Status201Created)]
    [ProducesResponseType(StatusCodes.Status400BadRequest)]
    public async Task<ActionResult<Product>> Create([FromBody] CreateProductRequest request, CancellationToken cancellationToken)
    {
        var created = await _repository.CreateAsync(request, cancellationToken);
        return CreatedAtAction(nameof(GetById), new { id = created.Id }, created);
    }

    /// <summary>Replace a product (full update).</summary>
    [HttpPut("{id:int}")]
    [ProducesResponseType(typeof(Product), StatusCodes.Status200OK)]
    [ProducesResponseType(StatusCodes.Status400BadRequest)]
    [ProducesResponseType(StatusCodes.Status404NotFound)]
    public async Task<ActionResult<Product>> Update(int id, [FromBody] UpdateProductRequest request, CancellationToken cancellationToken)
    {
        if (!await _repository.UpdateAsync(id, request, cancellationToken))
            return NotFound();

        var updated = await _repository.GetByIdAsync(id, cancellationToken);
        return Ok(updated);
    }

    /// <summary>Delete a product.</summary>
    [HttpDelete("{id:int}")]
    [ProducesResponseType(StatusCodes.Status204NoContent)]
    [ProducesResponseType(StatusCodes.Status404NotFound)]
    public async Task<IActionResult> Delete(int id, CancellationToken cancellationToken)
    {
        if (!await _repository.DeleteAsync(id, cancellationToken))
            return NotFound();

        return NoContent();
    }
}
