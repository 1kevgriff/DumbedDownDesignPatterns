using RepositoryPatternExample.Models;
using RepositoryPatternExample.Repositories.Interfaces;

namespace RepositoryPatternExample.Repositories.Implementations;

public class InMemoryProductRepository : IProductRepository
{
    private readonly List<Product> _products;
    private readonly ICategoryRepository _categoryRepository;
    private int _nextId = 1;

    public InMemoryProductRepository(ICategoryRepository categoryRepository)
    {
        _categoryRepository = categoryRepository;
        _products = GenerateSampleProducts();
    }

    public Task<IEnumerable<Product>> GetAllAsync()
    {
        return Task.FromResult(_products.AsEnumerable());
    }

    public Task<Product?> GetByIdAsync(int id)
    {
        var product = _products.FirstOrDefault(p => p.Id == id);
        return Task.FromResult(product);
    }

    public Task<Product> AddAsync(Product entity)
    {
        entity.Id = _nextId++;
        entity.CreatedAt = DateTime.UtcNow;
        _products.Add(entity);
        return Task.FromResult(entity);
    }

    public Task<Product> UpdateAsync(Product entity)
    {
        var existingProduct = _products.FirstOrDefault(p => p.Id == entity.Id);
        if (existingProduct != null)
        {
            existingProduct.Name = entity.Name;
            existingProduct.Description = entity.Description;
            existingProduct.Price = entity.Price;
            existingProduct.CategoryId = entity.CategoryId;
            existingProduct.IsActive = entity.IsActive;
            existingProduct.UpdatedAt = DateTime.UtcNow;
            return Task.FromResult(existingProduct);
        }
        throw new InvalidOperationException($"Product with ID {entity.Id} not found");
    }

    public Task<bool> DeleteAsync(int id)
    {
        var product = _products.FirstOrDefault(p => p.Id == id);
        if (product != null)
        {
            _products.Remove(product);
            return Task.FromResult(true);
        }
        return Task.FromResult(false);
    }

    public Task<bool> ExistsAsync(int id)
    {
        var exists = _products.Any(p => p.Id == id);
        return Task.FromResult(exists);
    }

    public Task<IEnumerable<Product>> GetByCategoryIdAsync(int categoryId)
    {
        var products = _products.Where(p => p.CategoryId == categoryId);
        return Task.FromResult(products);
    }

    public Task<IEnumerable<Product>> GetActiveProductsAsync()
    {
        var activeProducts = _products.Where(p => p.IsActive);
        return Task.FromResult(activeProducts);
    }

    public Task<IEnumerable<Product>> SearchByNameAsync(string name)
    {
        var products = _products.Where(p => p.Name.Contains(name, StringComparison.OrdinalIgnoreCase));
        return Task.FromResult(products);
    }

    public Task<IEnumerable<Product>> GetProductsInPriceRangeAsync(decimal minPrice, decimal maxPrice)
    {
        var products = _products.Where(p => p.Price >= minPrice && p.Price <= maxPrice);
        return Task.FromResult(products);
    }

    private List<Product> GenerateSampleProducts()
    {
        _nextId = 6; // Start from 6 since we're creating 5 sample products
        return new List<Product>
        {
            new Product
            {
                Id = 1,
                Name = "Laptop Pro 15",
                Description = "High-performance laptop for professionals",
                Price = 1299.99m,
                CategoryId = 1,
                IsActive = true,
                CreatedAt = DateTime.UtcNow.AddDays(-10)
            },
            new Product
            {
                Id = 2,
                Name = "Wireless Mouse",
                Description = "Ergonomic wireless mouse with precision tracking",
                Price = 49.99m,
                CategoryId = 1,
                IsActive = true,
                CreatedAt = DateTime.UtcNow.AddDays(-8)
            },
            new Product
            {
                Id = 3,
                Name = "Programming Book",
                Description = "Complete guide to modern software development",
                Price = 39.99m,
                CategoryId = 2,
                IsActive = true,
                CreatedAt = DateTime.UtcNow.AddDays(-5)
            },
            new Product
            {
                Id = 4,
                Name = "Mechanical Keyboard",
                Description = "Premium mechanical keyboard for coding",
                Price = 159.99m,
                CategoryId = 1,
                IsActive = true,
                CreatedAt = DateTime.UtcNow.AddDays(-3)
            },
            new Product
            {
                Id = 5,
                Name = "Design Patterns Guide",
                Description = "Essential patterns for software architecture",
                Price = 44.99m,
                CategoryId = 2,
                IsActive = false,
                CreatedAt = DateTime.UtcNow.AddDays(-1)
            }
        };
    }
}