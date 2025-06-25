using RepositoryPatternExample.Models;
using RepositoryPatternExample.Repositories.Interfaces;

namespace RepositoryPatternExample.Services;

public class CategoryService
{
    private readonly ICategoryRepository _categoryRepository;
    private readonly IProductRepository _productRepository;

    public CategoryService(ICategoryRepository categoryRepository, IProductRepository productRepository)
    {
        _categoryRepository = categoryRepository;
        _productRepository = productRepository;
    }

    public async Task<IEnumerable<Category>> GetAllCategoriesAsync()
    {
        return await _categoryRepository.GetAllAsync();
    }

    public async Task<Category?> GetCategoryByIdAsync(int id)
    {
        return await _categoryRepository.GetByIdAsync(id);
    }

    public async Task<Category> CreateCategoryAsync(Category category)
    {
        // Business logic: Validate name is not empty
        if (string.IsNullOrWhiteSpace(category.Name))
        {
            throw new ArgumentException("Category name cannot be empty");
        }

        // Business logic: Check for duplicate names
        var allCategories = await _categoryRepository.GetAllAsync();
        if (allCategories.Any(c => c.Name.Equals(category.Name, StringComparison.OrdinalIgnoreCase)))
        {
            throw new ArgumentException($"A category with the name '{category.Name}' already exists");
        }

        return await _categoryRepository.AddAsync(category);
    }

    public async Task<Category> UpdateCategoryAsync(Category category)
    {
        // Business logic: Check if category exists
        if (!await _categoryRepository.ExistsAsync(category.Id))
        {
            throw new ArgumentException($"Category with ID {category.Id} does not exist");
        }

        // Business logic: Validate name is not empty
        if (string.IsNullOrWhiteSpace(category.Name))
        {
            throw new ArgumentException("Category name cannot be empty");
        }

        // Business logic: Check for duplicate names (excluding current category)
        var allCategories = await _categoryRepository.GetAllAsync();
        if (allCategories.Any(c => c.Id != category.Id && c.Name.Equals(category.Name, StringComparison.OrdinalIgnoreCase)))
        {
            throw new ArgumentException($"A category with the name '{category.Name}' already exists");
        }

        return await _categoryRepository.UpdateAsync(category);
    }

    public async Task<bool> DeleteCategoryAsync(int id)
    {
        if (!await _categoryRepository.ExistsAsync(id))
        {
            throw new ArgumentException($"Category with ID {id} does not exist");
        }

        // Business logic: Check if category has products
        var productsInCategory = await _productRepository.GetByCategoryIdAsync(id);
        if (productsInCategory.Any())
        {
            throw new InvalidOperationException($"Cannot delete category with ID {id} because it contains products");
        }

        return await _categoryRepository.DeleteAsync(id);
    }

    public async Task<IEnumerable<Category>> GetActiveCategoriesAsync()
    {
        return await _categoryRepository.GetActiveCategoriesAsync();
    }

    public async Task<Category?> GetCategoryWithProductsAsync(int categoryId)
    {
        var category = await _categoryRepository.GetCategoryWithProductsAsync(categoryId);
        if (category != null)
        {
            // Load related products
            var products = await _productRepository.GetByCategoryIdAsync(categoryId);
            category.Products = products.ToList();
        }
        return category;
    }

    public async Task<IEnumerable<Category>> SearchCategoriesAsync(string searchTerm)
    {
        if (string.IsNullOrWhiteSpace(searchTerm))
        {
            return await _categoryRepository.GetAllAsync();
        }

        return await _categoryRepository.SearchByNameAsync(searchTerm);
    }
}