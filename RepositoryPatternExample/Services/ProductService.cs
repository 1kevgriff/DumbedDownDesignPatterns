using RepositoryPatternExample.Models;
using RepositoryPatternExample.Repositories.Interfaces;

namespace RepositoryPatternExample.Services;

public class ProductService
{
    private readonly IProductRepository _productRepository;
    private readonly ICategoryRepository _categoryRepository;

    public ProductService(IProductRepository productRepository, ICategoryRepository categoryRepository)
    {
        _productRepository = productRepository;
        _categoryRepository = categoryRepository;
    }

    public async Task<IEnumerable<Product>> GetAllProductsAsync()
    {
        return await _productRepository.GetAllAsync();
    }

    public async Task<Product?> GetProductByIdAsync(int id)
    {
        return await _productRepository.GetByIdAsync(id);
    }

    public async Task<Product> CreateProductAsync(Product product)
    {
        // Business logic: Validate that the category exists
        if (!await _categoryRepository.ExistsAsync(product.CategoryId))
        {
            throw new ArgumentException($"Category with ID {product.CategoryId} does not exist");
        }

        // Business logic: Ensure price is positive
        if (product.Price <= 0)
        {
            throw new ArgumentException("Product price must be greater than zero");
        }

        // Business logic: Validate name is not empty
        if (string.IsNullOrWhiteSpace(product.Name))
        {
            throw new ArgumentException("Product name cannot be empty");
        }

        return await _productRepository.AddAsync(product);
    }

    public async Task<Product> UpdateProductAsync(Product product)
    {
        // Business logic: Check if product exists
        if (!await _productRepository.ExistsAsync(product.Id))
        {
            throw new ArgumentException($"Product with ID {product.Id} does not exist");
        }

        // Business logic: Validate that the category exists
        if (!await _categoryRepository.ExistsAsync(product.CategoryId))
        {
            throw new ArgumentException($"Category with ID {product.CategoryId} does not exist");
        }

        // Business logic: Ensure price is positive
        if (product.Price <= 0)
        {
            throw new ArgumentException("Product price must be greater than zero");
        }

        return await _productRepository.UpdateAsync(product);
    }

    public async Task<bool> DeleteProductAsync(int id)
    {
        if (!await _productRepository.ExistsAsync(id))
        {
            throw new ArgumentException($"Product with ID {id} does not exist");
        }

        return await _productRepository.DeleteAsync(id);
    }

    public async Task<IEnumerable<Product>> GetProductsByCategoryAsync(int categoryId)
    {
        // Business logic: Validate category exists
        if (!await _categoryRepository.ExistsAsync(categoryId))
        {
            throw new ArgumentException($"Category with ID {categoryId} does not exist");
        }

        return await _productRepository.GetByCategoryIdAsync(categoryId);
    }

    public async Task<IEnumerable<Product>> GetActiveProductsAsync()
    {
        return await _productRepository.GetActiveProductsAsync();
    }

    public async Task<IEnumerable<Product>> SearchProductsAsync(string searchTerm)
    {
        if (string.IsNullOrWhiteSpace(searchTerm))
        {
            return await _productRepository.GetAllAsync();
        }

        return await _productRepository.SearchByNameAsync(searchTerm);
    }

    public async Task<IEnumerable<Product>> GetProductsInPriceRangeAsync(decimal minPrice, decimal maxPrice)
    {
        // Business logic: Validate price range
        if (minPrice < 0 || maxPrice < 0)
        {
            throw new ArgumentException("Price values cannot be negative");
        }

        if (minPrice > maxPrice)
        {
            throw new ArgumentException("Minimum price cannot be greater than maximum price");
        }

        return await _productRepository.GetProductsInPriceRangeAsync(minPrice, maxPrice);
    }
}