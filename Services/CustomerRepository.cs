using APICRUD.Data;
using APICRUD.IServices;
using APICRUD.Models;
using Dapper;
using Microsoft.Data.SqlClient;

namespace APICRUD.Services;

public class CustomerRepository : ICustomerRepository
{
    private readonly DatabaseSettings _database;

    public CustomerRepository(DatabaseSettings database)
    {
        _database = database;
    }

    public async Task<IReadOnlyList<Customer>> GetAllAsync(CancellationToken cancellationToken = default)
    {
        const string sql = """
            SELECT CustomerId,
                   CustomerCode,
                   CustomerName,
                   Email,
                   Phone,
                   Address,
                   City,
                   Country,
                   IsActive
            FROM Customer
            ORDER BY CustomerId;
            """;

        await using var connection = new SqlConnection(_database.ConnectionString);
        var rows = await connection.QueryAsync<Customer>(
            new CommandDefinition(sql, cancellationToken: cancellationToken));

        return rows.AsList();
    }

    public async Task<Customer?> GetByIdAsync(int customerId, CancellationToken cancellationToken = default)
    {
        const string sql = """
            SELECT CustomerId,
                   CustomerCode,
                   CustomerName,
                   Email,
                   Phone,
                   Address,
                   City,
                   Country,
                   IsActive
            FROM Customer
            WHERE CustomerId = @CustomerId;
            """;

        await using var connection = new SqlConnection(_database.ConnectionString);
        return await connection.QuerySingleOrDefaultAsync<Customer>(
            new CommandDefinition(sql, new { CustomerId = customerId }, cancellationToken: cancellationToken));
    }

    public async Task<Customer> CreateAsync(CreateCustomerRequest request, CancellationToken cancellationToken = default)
    {
        const string sql = """
            INSERT INTO Customer (
                CustomerCode,
                CustomerName,
                Email,
                Phone,
                Address,
                City,
                Country,
                IsActive)
            OUTPUT INSERTED.CustomerId,
                   INSERTED.CustomerCode,
                   INSERTED.CustomerName,
                   INSERTED.Email,
                   INSERTED.Phone,
                   INSERTED.Address,
                   INSERTED.City,
                   INSERTED.Country,
                   INSERTED.IsActive
            VALUES (
                @CustomerCode,
                @CustomerName,
                @Email,
                @Phone,
                @Address,
                @City,
                @Country,
                @IsActive);
            """;

        var parameters = new
        {
            CustomerCode = request.CustomerCode.Trim(),
            CustomerName = request.CustomerName.Trim(),
            Email = NullIfWhiteSpace(request.Email),
            Phone = NullIfWhiteSpace(request.Phone),
            Address = NullIfWhiteSpace(request.Address),
            City = NullIfWhiteSpace(request.City),
            Country = NullIfWhiteSpace(request.Country),
            request.IsActive
        };

        await using var connection = new SqlConnection(_database.ConnectionString);
        return await connection.QuerySingleAsync<Customer>(
            new CommandDefinition(sql, parameters, cancellationToken: cancellationToken));
    }

    public async Task<bool> UpdateAsync(int customerId, UpdateCustomerRequest request, CancellationToken cancellationToken = default)
    {
        const string sql = """
            UPDATE Customer
            SET CustomerCode = @CustomerCode,
                CustomerName = @CustomerName,
                Email = @Email,
                Phone = @Phone,
                Address = @Address,
                City = @City,
                Country = @Country,
                IsActive = @IsActive
            WHERE CustomerId = @CustomerId;
            """;

        var parameters = new
        {
            CustomerId = customerId,
            CustomerCode = request.CustomerCode.Trim(),
            CustomerName = request.CustomerName.Trim(),
            Email = NullIfWhiteSpace(request.Email),
            Phone = NullIfWhiteSpace(request.Phone),
            Address = NullIfWhiteSpace(request.Address),
            City = NullIfWhiteSpace(request.City),
            Country = NullIfWhiteSpace(request.Country),
            request.IsActive
        };

        await using var connection = new SqlConnection(_database.ConnectionString);
        var rows = await connection.ExecuteAsync(
            new CommandDefinition(sql, parameters, cancellationToken: cancellationToken));

        return rows > 0;
    }

    public async Task<bool> DeleteAsync(int customerId, CancellationToken cancellationToken = default)
    {
        const string sql = """
            DELETE FROM Customer
            WHERE CustomerId = @CustomerId;
            """;

        await using var connection = new SqlConnection(_database.ConnectionString);
        var rows = await connection.ExecuteAsync(
            new CommandDefinition(sql, new { CustomerId = customerId }, cancellationToken: cancellationToken));

        return rows > 0;
    }

    public async Task<Customer?> GetByCustomerNameAsync(string customerName, CancellationToken cancellationToken = default)
    {
        const string sql = """
            SELECT TOP (1)
                   CustomerId,
                   CustomerCode,
                   CustomerName,
                   Email,
                   Phone,
                   Address,
                   City,
                   Country,
                   IsActive
            FROM Customer
            WHERE CustomerName = @CustomerName;
            """;

        await using var connection = new SqlConnection(_database.ConnectionString);
        return await connection.QueryFirstOrDefaultAsync<Customer>(
            new CommandDefinition(
                sql,
                new { CustomerName = customerName.Trim() },
                cancellationToken: cancellationToken));
    }

    private static string? NullIfWhiteSpace(string? value) =>
        string.IsNullOrWhiteSpace(value) ? null : value.Trim();
}
