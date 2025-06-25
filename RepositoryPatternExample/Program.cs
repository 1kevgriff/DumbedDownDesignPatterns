using RepositoryPatternExample.Repositories.Interfaces;
using RepositoryPatternExample.Repositories.Implementations;
using RepositoryPatternExample.Services;

var builder = WebApplication.CreateBuilder(args);

// Add services to the container
builder.Services.AddControllers();

// Register repositories
builder.Services.AddSingleton<ICategoryRepository, InMemoryCategoryRepository>();
builder.Services.AddSingleton<IProductRepository, InMemoryProductRepository>();

// Register services
builder.Services.AddScoped<ProductService>();
builder.Services.AddScoped<CategoryService>();

// Add API documentation
builder.Services.AddEndpointsApiExplorer();
builder.Services.AddSwaggerGen(c =>
{
    c.SwaggerDoc("v1", new() { 
        Title = "Repository Pattern Example API", 
        Version = "v1",
        Description = "A .NET 9 Web API demonstrating the Repository Pattern with in-memory data"
    });
});

var app = builder.Build();

// Configure the HTTP request pipeline
if (app.Environment.IsDevelopment())
{
    app.UseSwagger();
    app.UseSwaggerUI(c =>
    {
        c.SwaggerEndpoint("/swagger/v1/swagger.json", "Repository Pattern Example API v1");
        c.RoutePrefix = string.Empty; // Serve Swagger UI at the app's root
    });
}

app.UseHttpsRedirection();
app.UseAuthorization();
app.MapControllers();

app.Run();