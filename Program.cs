using APICRUD.Data;
using APICRUD.IServices;
using APICRUD.Services;

var builder = WebApplication.CreateBuilder(args);

builder.Services.AddControllers();
builder.Services.AddEndpointsApiExplorer();
builder.Services.AddSwaggerGen(options =>
{
    options.SwaggerDoc("v1", new()
    {
        Title = "Catalog API",
        Version = "v1",
        Description = "REST CRUD for Product and Customer tables on SQL Server using Dapper."
    });
});

var connectionString = SqlServerConnectionResolver.Resolve(builder.Configuration);
builder.Services.AddSingleton(new DatabaseSettings { ConnectionString = connectionString });
builder.Services.AddScoped<IProductRepository, ProductRepository>();
builder.Services.AddScoped<ICustomerRepository, CustomerRepository>();

var app = builder.Build();

if (app.Environment.IsDevelopment())
{
    app.UseSwagger();
    app.UseSwaggerUI();
}

app.UseHttpsRedirection();

app.UseAuthorization();

app.MapControllers();

app.Run();
