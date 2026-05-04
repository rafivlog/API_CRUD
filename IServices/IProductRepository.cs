using APICRUD.Models;

namespace APICRUD.IServices;

public interface IProductRepository
{
    Task<IReadOnlyList<Product>> GetAllAsync(CancellationToken cancellationToken = default);

    Task<Product?> GetByIdAsync(int id, CancellationToken cancellationToken = default);

    Task<Product> CreateAsync(CreateProductRequest request, CancellationToken cancellationToken = default);

    Task<bool> UpdateAsync(int id, UpdateProductRequest request, CancellationToken cancellationToken = default);

    Task<bool> DeleteAsync(int id, CancellationToken cancellationToken = default);
}
