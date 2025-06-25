using Microsoft.AspNetCore.Mvc;
using RepositoryPatternExample.Models;
using RepositoryPatternExample.Services;

namespace RepositoryPatternExample.Controllers;

[ApiController]
[Route("api/[controller]")]
public class CategoriesController : ControllerBase
{
    private readonly CategoryService _categoryService;

    public CategoriesController(CategoryService categoryService)
    {
        _categoryService = categoryService;
    }

    /// <summary>
    /// Get all categories
    /// </summary>
    [HttpGet]
    public async Task<ActionResult<IEnumerable<Category>>> GetCategories()
    {
        try
        {
            var categories = await _categoryService.GetAllCategoriesAsync();
            return Ok(categories);
        }
        catch (Exception ex)
        {
            return StatusCode(500, $"Internal server error: {ex.Message}");
        }
    }

    /// <summary>
    /// Get a specific category by ID
    /// </summary>
    [HttpGet("{id}")]
    public async Task<ActionResult<Category>> GetCategory(int id)
    {
        try
        {
            var category = await _categoryService.GetCategoryByIdAsync(id);
            if (category == null)
            {
                return NotFound($"Category with ID {id} not found");
            }
            return Ok(category);
        }
        catch (Exception ex)
        {
            return StatusCode(500, $"Internal server error: {ex.Message}");
        }
    }

    /// <summary>
    /// Create a new category
    /// </summary>
    [HttpPost]
    public async Task<ActionResult<Category>> CreateCategory(Category category)
    {
        try
        {
            var createdCategory = await _categoryService.CreateCategoryAsync(category);
            return CreatedAtAction(nameof(GetCategory), new { id = createdCategory.Id }, createdCategory);
        }
        catch (ArgumentException ex)
        {
            return BadRequest(ex.Message);
        }
        catch (Exception ex)
        {
            return StatusCode(500, $"Internal server error: {ex.Message}");
        }
    }

    /// <summary>
    /// Update an existing category
    /// </summary>
    [HttpPut("{id}")]
    public async Task<ActionResult<Category>> UpdateCategory(int id, Category category)
    {
        if (id != category.Id)
        {
            return BadRequest("Category ID in URL does not match category ID in body");
        }

        try
        {
            var updatedCategory = await _categoryService.UpdateCategoryAsync(category);
            return Ok(updatedCategory);
        }
        catch (ArgumentException ex)
        {
            return BadRequest(ex.Message);
        }
        catch (Exception ex)
        {
            return StatusCode(500, $"Internal server error: {ex.Message}");
        }
    }

    /// <summary>
    /// Delete a category
    /// </summary>
    [HttpDelete("{id}")]
    public async Task<ActionResult> DeleteCategory(int id)
    {
        try
        {
            var deleted = await _categoryService.DeleteCategoryAsync(id);
            if (!deleted)
            {
                return NotFound($"Category with ID {id} not found");
            }
            return NoContent();
        }
        catch (ArgumentException ex)
        {
            return BadRequest(ex.Message);
        }
        catch (InvalidOperationException ex)
        {
            return Conflict(ex.Message);
        }
        catch (Exception ex)
        {
            return StatusCode(500, $"Internal server error: {ex.Message}");
        }
    }

    /// <summary>
    /// Get only active categories
    /// </summary>
    [HttpGet("active")]
    public async Task<ActionResult<IEnumerable<Category>>> GetActiveCategories()
    {
        try
        {
            var categories = await _categoryService.GetActiveCategoriesAsync();
            return Ok(categories);
        }
        catch (Exception ex)
        {
            return StatusCode(500, $"Internal server error: {ex.Message}");
        }
    }

    /// <summary>
    /// Get a category with its products
    /// </summary>
    [HttpGet("{id}/with-products")]
    public async Task<ActionResult<Category>> GetCategoryWithProducts(int id)
    {
        try
        {
            var category = await _categoryService.GetCategoryWithProductsAsync(id);
            if (category == null)
            {
                return NotFound($"Category with ID {id} not found");
            }
            return Ok(category);
        }
        catch (Exception ex)
        {
            return StatusCode(500, $"Internal server error: {ex.Message}");
        }
    }

    /// <summary>
    /// Search categories by name
    /// </summary>
    [HttpGet("search")]
    public async Task<ActionResult<IEnumerable<Category>>> SearchCategories([FromQuery] string? q)
    {
        try
        {
            var categories = await _categoryService.SearchCategoriesAsync(q ?? string.Empty);
            return Ok(categories);
        }
        catch (Exception ex)
        {
            return StatusCode(500, $"Internal server error: {ex.Message}");
        }
    }
}