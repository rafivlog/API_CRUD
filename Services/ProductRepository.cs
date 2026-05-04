using APICRUD.Data;
using APICRUD.IServices;
using APICRUD.Models;
using Dapper;
using Microsoft.Data.SqlClient;

namespace APICRUD.Services;

public class ProductRepository : IProductRepository
{
    private readonly DatabaseSettings _database;

    public ProductRepository(DatabaseSettings database)
    {
        _database = database;
    }

    public async Task<IReadOnlyList<Product>> GetAllAsync(CancellationToken cancellationToken = default)
    {
        const string sql = """
            SELECT ID AS Id, ProductName, Quantity, Price, CreatedDate
            FROM Product
            ORDER BY ID;
            """;

        await using var connection = new SqlConnection(_database.ConnectionString);
        var rows = await connection.QueryAsync<Product>(
            new CommandDefinition(sql, cancellationToken: cancellationToken));

        return rows.AsList();
    }

    public async Task<Product?> GetByIdAsync(int id, CancellationToken cancellationToken = default)
    {
        const string sql = """
            SELECT ID AS Id, ProductName, Quantity, Price, CreatedDate
            FROM Product
            WHERE ID = @Id;
            """;

        await using var connection = new SqlConnection(_database.ConnectionString);
        return await connection.QuerySingleOrDefaultAsync<Product>(
            new CommandDefinition(sql, new { Id = id }, cancellationToken: cancellationToken));
    }

    public async Task<Product> CreateAsync(CreateProductRequest request, CancellationToken cancellationToken = default)
    {
        const string sql = """
            INSERT INTO Product (ProductName, Quantity, Price)
            OUTPUT INSERTED.ID AS Id,
                   INSERTED.ProductName,
                   INSERTED.Quantity,
                   INSERTED.Price,
                   INSERTED.CreatedDate
            VALUES (@ProductName, @Quantity, @Price);
            """;

        var parameters = new
        {
            ProductName = request.ProductName.Trim(),
            request.Quantity,
            request.Price
        };

        await using var connection = new SqlConnection(_database.ConnectionString);
        return await connection.QuerySingleAsync<Product>(
            new CommandDefinition(sql, parameters, cancellationToken: cancellationToken));
    }

    public async Task<bool> UpdateAsync(int id, UpdateProductRequest request, CancellationToken cancellationToken = default)
    {
        const string sql = """
            UPDATE Product
            SET ProductName = @ProductName,
                Quantity = @Quantity,
                Price = @Price
            WHERE ID = @Id;
            """;

        var parameters = new
        {
            Id = id,
            ProductName = request.ProductName.Trim(),
            request.Quantity,
            request.Price
        };

        await using var connection = new SqlConnection(_database.ConnectionString);
        var rows = await connection.ExecuteAsync(
            new CommandDefinition(sql, parameters, cancellationToken: cancellationToken));

        return rows > 0;
    }

    public async Task<bool> DeleteAsync(int id, CancellationToken cancellationToken = default)
    {
        const string sql = """
            DELETE FROM Product
            WHERE ID = @Id;
            """;

        await using var connection = new SqlConnection(_database.ConnectionString);
        var rows = await connection.ExecuteAsync(
            new CommandDefinition(sql, new { Id = id }, cancellationToken: cancellationToken));

        return rows > 0;
    }
}
