using APICRUD.IServices;
using APICRUD.Models;
using Microsoft.AspNetCore.Mvc;

namespace APICRUD.Controllers;

[ApiController]
[Route("api/customers")]
public class CustomersController : ControllerBase
{
    private readonly ICustomerRepository _repository;

    public CustomersController(ICustomerRepository repository)
    {
        _repository = repository;
    }

    /// <summary>List all customers.</summary>
    [HttpGet]
    [ProducesResponseType(typeof(IReadOnlyList<Customer>), StatusCodes.Status200OK)]
    public async Task<ActionResult<IReadOnlyList<Customer>>> GetAll(CancellationToken cancellationToken)
    {
        var items = await _repository.GetAllAsync(cancellationToken);
        return Ok(items);
    }

    /// <summary>Get a customer by id.</summary>
    [HttpGet("{customerId:int}")]
    [ProducesResponseType(typeof(Customer), StatusCodes.Status200OK)]
    [ProducesResponseType(StatusCodes.Status404NotFound)]
    public async Task<ActionResult<Customer>> GetById(int customerId, CancellationToken cancellationToken)
    {
        var customer = await _repository.GetByIdAsync(customerId, cancellationToken);
        if (customer is null)
            return NotFound();

        return Ok(customer);
    }

    /// <summary>Get the first customer with an exact matching name (e.g. GET .../by-name?customerName=Acme).</summary>
    [HttpGet("customer")]
    [ProducesResponseType(typeof(Customer), StatusCodes.Status200OK)]
    [ProducesResponseType(StatusCodes.Status400BadRequest)]
    [ProducesResponseType(StatusCodes.Status404NotFound)]
    public async Task<ActionResult<Customer>> GetByCustomerName([FromQuery] string customerName, CancellationToken cancellationToken)
    {
        if (string.IsNullOrWhiteSpace(customerName))
            return BadRequest("Query parameter 'customerName' is required.");

        var customer = await _repository.GetByCustomerNameAsync(customerName, cancellationToken);
        if (customer is null)
            return NotFound();

        return Ok(customer);
    }

    /// <summary>Create a customer.</summary>
    [HttpPost]
    [ProducesResponseType(typeof(Customer), StatusCodes.Status201Created)]
    [ProducesResponseType(StatusCodes.Status400BadRequest)]
    public async Task<ActionResult<Customer>> Create([FromBody] CreateCustomerRequest request, CancellationToken cancellationToken)
    {
        var created = await _repository.CreateAsync(request, cancellationToken);
        return CreatedAtAction(nameof(GetById), new { customerId = created.CustomerId }, created);
    }


    /// <summary>Replace a customer (full update).</summary>
    [HttpPut("{customerId:int}")]
    [ProducesResponseType(typeof(Customer), StatusCodes.Status200OK)]
    [ProducesResponseType(StatusCodes.Status400BadRequest)]
    [ProducesResponseType(StatusCodes.Status404NotFound)]
    public async Task<ActionResult<Customer>> Update(int customerId, [FromBody] UpdateCustomerRequest request, CancellationToken cancellationToken)
    {
        if (!await _repository.UpdateAsync(customerId, request, cancellationToken))
            return NotFound();

        var updated = await _repository.GetByIdAsync(customerId, cancellationToken);
        return Ok(updated);
    }

    /// <summary>Delete a customer.</summary>
    [HttpDelete("{customerId:int}")]
    [ProducesResponseType(StatusCodes.Status204NoContent)]
    [ProducesResponseType(StatusCodes.Status404NotFound)]
    public async Task<IActionResult> Delete(int customerId, CancellationToken cancellationToken)
    {
        if (!await _repository.DeleteAsync(customerId, cancellationToken))
            return NotFound();

        return NoContent();
    }
}
