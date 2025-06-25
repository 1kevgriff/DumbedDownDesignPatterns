using RepositoryPatternExample.Models;

namespace RepositoryPatternExample.Repositories.Interfaces;

public interface ICategoryRepository : IRepository<Category>
{
    Task<IEnumerable<Category>> GetActiveCategoriesAsync();
    Task<Category?> GetCategoryWithProductsAsync(int categoryId);
    Task<IEnumerable<Category>> SearchByNameAsync(string name);
}