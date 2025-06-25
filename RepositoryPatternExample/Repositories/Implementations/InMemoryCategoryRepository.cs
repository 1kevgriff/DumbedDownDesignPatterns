using RepositoryPatternExample.Models;
using RepositoryPatternExample.Repositories.Interfaces;

namespace RepositoryPatternExample.Repositories.Implementations;

public class InMemoryCategoryRepository : ICategoryRepository
{
    private readonly List<Category> _categories;
    private int _nextId = 1;

    public InMemoryCategoryRepository()
    {
        _categories = GenerateSampleCategories();
    }

    public Task<IEnumerable<Category>> GetAllAsync()
    {
        return Task.FromResult(_categories.AsEnumerable());
    }

    public Task<Category?> GetByIdAsync(int id)
    {
        var category = _categories.FirstOrDefault(c => c.Id == id);
        return Task.FromResult(category);
    }

    public Task<Category> AddAsync(Category entity)
    {
        entity.Id = _nextId++;
        entity.CreatedAt = DateTime.UtcNow;
        _categories.Add(entity);
        return Task.FromResult(entity);
    }

    public Task<Category> UpdateAsync(Category entity)
    {
        var existingCategory = _categories.FirstOrDefault(c => c.Id == entity.Id);
        if (existingCategory != null)
        {
            existingCategory.Name = entity.Name;
            existingCategory.Description = entity.Description;
            existingCategory.IsActive = entity.IsActive;
            return Task.FromResult(existingCategory);
        }
        throw new InvalidOperationException($"Category with ID {entity.Id} not found");
    }

    public Task<bool> DeleteAsync(int id)
    {
        var category = _categories.FirstOrDefault(c => c.Id == id);
        if (category != null)
        {
            _categories.Remove(category);
            return Task.FromResult(true);
        }
        return Task.FromResult(false);
    }

    public Task<bool> ExistsAsync(int id)
    {
        var exists = _categories.Any(c => c.Id == id);
        return Task.FromResult(exists);
    }

    public Task<IEnumerable<Category>> GetActiveCategoriesAsync()
    {
        var activeCategories = _categories.Where(c => c.IsActive);
        return Task.FromResult(activeCategories);
    }

    public Task<Category?> GetCategoryWithProductsAsync(int categoryId)
    {
        // In a real implementation, this would include related products
        // For this demo, we'll just return the category
        var category = _categories.FirstOrDefault(c => c.Id == categoryId);
        return Task.FromResult(category);
    }

    public Task<IEnumerable<Category>> SearchByNameAsync(string name)
    {
        var categories = _categories.Where(c => c.Name.Contains(name, StringComparison.OrdinalIgnoreCase));
        return Task.FromResult(categories);
    }

    private List<Category> GenerateSampleCategories()
    {
        _nextId = 4; // Start from 4 since we're creating 3 sample categories
        return new List<Category>
        {
            new Category
            {
                Id = 1,
                Name = "Electronics",
                Description = "Electronic devices and accessories",
                IsActive = true,
                CreatedAt = DateTime.UtcNow.AddDays(-15)
            },
            new Category
            {
                Id = 2,
                Name = "Books",
                Description = "Educational and reference books",
                IsActive = true,
                CreatedAt = DateTime.UtcNow.AddDays(-12)
            },
            new Category
            {
                Id = 3,
                Name = "Furniture",
                Description = "Office and home furniture",
                IsActive = false,
                CreatedAt = DateTime.UtcNow.AddDays(-8)
            }
        };
    }
}