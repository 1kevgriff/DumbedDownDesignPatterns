# .NET 9 Repository Pattern Example

I have successfully created a comprehensive .NET 9 Web API application that demonstrates the **Repository Pattern** using in-memory data instead of a real database.

## What Was Created

### Project Structure
```
RepositoryPatternExample/
├── Models/                          # Domain entities
│   ├── Product.cs                   # Product domain model
│   └── Category.cs                  # Category domain model
├── Repositories/
│   ├── Interfaces/                  # Repository contracts
│   │   ├── IRepository.cs           # Generic repository interface
│   │   ├── IProductRepository.cs    # Product-specific operations
│   │   └── ICategoryRepository.cs   # Category-specific operations
│   └── Implementations/             # Concrete implementations
│       ├── InMemoryProductRepository.cs
│       └── InMemoryCategoryRepository.cs
├── Services/                        # Business logic layer
│   ├── ProductService.cs            # Product business logic
│   └── CategoryService.cs           # Category business logic
├── Controllers/                     # Web API controllers
│   ├── ProductsController.cs        # Product endpoints
│   └── CategoriesController.cs      # Category endpoints
├── Program.cs                       # Application entry point
├── RepositoryPatternExample.csproj  # Project file
└── README.md                        # Detailed documentation
```

## Key Repository Pattern Features Demonstrated

### 1. **Separation of Concerns**
- **Data Access Layer**: Repository interfaces and implementations
- **Business Logic Layer**: Service classes with validation
- **Presentation Layer**: Web API controllers

### 2. **Generic Repository Interface**
```csharp
public interface IRepository<T> where T : class
{
    Task<IEnumerable<T>> GetAllAsync();
    Task<T?> GetByIdAsync(int id);
    Task<T> AddAsync(T entity);
    Task<T> UpdateAsync(T entity);
    Task<bool> DeleteAsync(int id);
    Task<bool> ExistsAsync(int id);
}
```

### 3. **Specific Repository Extensions**
- `IProductRepository`: Adds product-specific methods
- `ICategoryRepository`: Adds category-specific methods

### 4. **In-Memory Implementations**
- Uses `List<T>` collections to simulate database operations
- Pre-populated with sample data for immediate testing
- Thread-safe operations with proper async/await patterns

### 5. **Dependency Injection Setup**
```csharp
// Register repositories as singletons (for in-memory persistence)
builder.Services.AddSingleton<ICategoryRepository, InMemoryCategoryRepository>();
builder.Services.AddSingleton<IProductRepository, InMemoryProductRepository>();

// Register services as scoped (per request)
builder.Services.AddScoped<ProductService>();
builder.Services.AddScoped<CategoryService>();
```

## Sample Data Included

### Categories
1. **Electronics** - Electronic devices and accessories
2. **Books** - Educational and reference books  
3. **Furniture** - Office and home furniture (inactive)

### Products
1. **Laptop Pro 15** - High-performance laptop ($1299.99, Electronics)
2. **Wireless Mouse** - Ergonomic wireless mouse ($49.99, Electronics)
3. **Programming Book** - Complete guide to software development ($39.99, Books)
4. **Mechanical Keyboard** - Premium keyboard for coding ($159.99, Electronics)
5. **Design Patterns Guide** - Essential patterns book ($44.99, Books, inactive)

## API Endpoints Available

### Products API
- `GET /api/products` - Get all products
- `GET /api/products/{id}` - Get product by ID
- `POST /api/products` - Create new product
- `PUT /api/products/{id}` - Update product
- `DELETE /api/products/{id}` - Delete product
- `GET /api/products/category/{categoryId}` - Get products by category
- `GET /api/products/active` - Get only active products
- `GET /api/products/search?q={searchTerm}` - Search products by name
- `GET /api/products/price-range?minPrice={min}&maxPrice={max}` - Get products in price range

### Categories API
- `GET /api/categories` - Get all categories
- `GET /api/categories/{id}` - Get category by ID
- `POST /api/categories` - Create new category
- `PUT /api/categories/{id}` - Update category
- `DELETE /api/categories/{id}` - Delete category
- `GET /api/categories/active` - Get only active categories
- `GET /api/categories/{id}/with-products` - Get category with its products
- `GET /api/categories/search?q={searchTerm}` - Search categories by name

## How to Run

1. **Prerequisites**: .NET 9 SDK
2. **Navigate to project**: `cd RepositoryPatternExample`
3. **Build**: `dotnet build`
4. **Run**: `dotnet run`
5. **Access**: 
   - Swagger UI: `https://localhost:5001` 
   - API: `https://localhost:5001/api`

## Key Benefits Demonstrated

1. **Testability**: Easy to mock repositories for unit testing
2. **Maintainability**: Clean separation between data access and business logic
3. **Flexibility**: Easy to switch from in-memory to database implementations
4. **Scalability**: Service layer can orchestrate multiple repositories
5. **Modern .NET 9 Features**: Uses latest C# language features and ASP.NET Core

## Real-World Usage Notes

This example uses in-memory data for demonstration. In production, you would:
- Replace with Entity Framework Core repositories
- Add Unit of Work pattern for transactions
- Implement proper error handling and logging
- Add authentication/authorization
- Use DTOs and AutoMapper
- Add caching layers
- Implement validation attributes

The Repository pattern provides excellent foundation for building maintainable, testable, and scalable .NET applications while keeping data access concerns properly separated from business logic.

## Build Status
✅ **Successfully Built**: The project compiles without errors using .NET 9.0.100