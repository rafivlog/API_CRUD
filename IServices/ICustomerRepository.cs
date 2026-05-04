using APICRUD.Models;

namespace APICRUD.IServices;

public interface ICustomerRepository
{
    Task<IReadOnlyList<Customer>> GetAllAsync(CancellationToken cancellationToken = default);

    Task<Customer?> GetByIdAsync(int customerId, CancellationToken cancellationToken = default);

    /// <summary>First match by exact customer name (names are not required to be unique).</summary>
    Task<Customer?> GetByCustomerNameAsync(string customerName, CancellationToken cancellationToken = default);

    Task<Customer> CreateAsync(CreateCustomerRequest request, CancellationToken cancellationToken = default);

    Task<bool> UpdateAsync(int customerId, UpdateCustomerRequest request, CancellationToken cancellationToken = default);

    Task<bool> DeleteAsync(int customerId, CancellationToken cancellationToken = default);
}
