# Repository Pattern Example - .NET 9

This project demonstrates the **Repository Pattern** implementation in a .NET 9 Web API application using in-memory data instead of a real database.

## What is the Repository Pattern?

The Repository pattern encapsulates the logic needed to access data sources. It centralizes common data access functionality, providing better maintainability and decoupling the infrastructure or technology used to access databases from the domain model layer.

### Key Benefits

- **Separation of Concerns**: Business logic is separated from data access logic
- **Testability**: Easy to mock repositories for unit testing
- **Maintainability**: Changes to data access logic don't affect business logic
- **Flexibility**: Easy to switch between different data sources (database, file, web service, etc.)

## Project Structure

```
RepositoryPatternExample/
├── Models/                          # Domain entities
│   ├── Product.cs
│   └── Category.cs
├── Repositories/
│   ├── Interfaces/                  # Repository contracts
│   │   ├── IRepository.cs           # Generic repository interface
│   │   ├── IProductRepository.cs    # Product-specific operations
│   │   └── ICategoryRepository.cs   # Category-specific operations
│   └── Implementations/             # Concrete implementations
│       ├── InMemoryProductRepository.cs
│       └── InMemoryCategoryRepository.cs
├── Services/                        # Business logic layer
│   ├── ProductService.cs
│   └── CategoryService.cs
├── Controllers/                     # Web API controllers
│   ├── ProductsController.cs
│   └── CategoriesController.cs
└── Program.cs                       # Application entry point
```

## Key Components

### 1. Generic Repository Interface (`IRepository<T>`)
Defines basic CRUD operations that all repositories should implement:
- `GetAllAsync()`
- `GetByIdAsync(int id)`
- `AddAsync(T entity)`
- `UpdateAsync(T entity)`
- `DeleteAsync(int id)`
- `ExistsAsync(int id)`

### 2. Specific Repository Interfaces
Extend the generic repository with entity-specific operations:
- `IProductRepository`: Adds methods like `GetByCategoryIdAsync()`, `GetActiveProductsAsync()`
- `ICategoryRepository`: Adds methods like `GetActiveCategoriesAsync()`, `GetCategoryWithProductsAsync()`

### 3. In-Memory Implementations
Concrete implementations using `List<T>` collections to simulate database operations:
- `InMemoryProductRepository`: Contains sample product data
- `InMemoryCategoryRepository`: Contains sample category data

### 4. Service Layer
Contains business logic and orchestrates repository operations:
- `ProductService`: Validates business rules before calling repository methods
- `CategoryService`: Handles category-specific business logic

### 5. Controllers
Web API endpoints that use services to handle HTTP requests:
- `ProductsController`: RESTful endpoints for product operations
- `CategoriesController`: RESTful endpoints for category operations

## Running the Application

1. **Prerequisites**: Ensure you have .NET 9 SDK installed

2. **Build and Run**:
   ```bash
   cd RepositoryPatternExample
   dotnet build
   dotnet run
   ```

3. **Access the API**:
   - Swagger UI: `https://localhost:5001` or `http://localhost:5000`
   - API Base URL: `https://localhost:5001/api`

## API Endpoints

### Products
- `GET /api/products` - Get all products
- `GET /api/products/{id}` - Get product by ID
- `POST /api/products` - Create new product
- `PUT /api/products/{id}` - Update product
- `DELETE /api/products/{id}` - Delete product
- `GET /api/products/category/{categoryId}` - Get products by category
- `GET /api/products/active` - Get only active products
- `GET /api/products/search?q={searchTerm}` - Search products by name
- `GET /api/products/price-range?minPrice={min}&maxPrice={max}` - Get products in price range

### Categories
- `GET /api/categories` - Get all categories
- `GET /api/categories/{id}` - Get category by ID
- `POST /api/categories` - Create new category
- `PUT /api/categories/{id}` - Update category
- `DELETE /api/categories/{id}` - Delete category
- `GET /api/categories/active` - Get only active categories
- `GET /api/categories/{id}/with-products` - Get category with its products
- `GET /api/categories/search?q={searchTerm}` - Search categories by name

## Sample Data

The application comes with pre-populated sample data:

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

## Dependency Injection Setup

The application uses .NET's built-in dependency injection container:

```csharp
// Register repositories as singletons (in-memory data persistence)
builder.Services.AddSingleton<ICategoryRepository, InMemoryCategoryRepository>();
builder.Services.AddSingleton<IProductRepository, InMemoryProductRepository>();

// Register services as scoped (per request)
builder.Services.AddScoped<ProductService>();
builder.Services.AddScoped<CategoryService>();
```

## Real-World Usage

In a production application, you would:

1. **Replace in-memory repositories** with Entity Framework Core implementations
2. **Add a Unit of Work pattern** for transaction management
3. **Implement proper error handling** and logging
4. **Add validation attributes** to models
5. **Use AutoMapper** for DTO mapping
6. **Add authentication and authorization**
7. **Implement caching** for better performance

## Testing Benefits

The Repository pattern makes testing easier:

```csharp
// Easy to mock repositories for unit testing
var mockProductRepo = new Mock<IProductRepository>();
var mockCategoryRepo = new Mock<ICategoryRepository>();
var productService = new ProductService(mockProductRepo.Object, mockCategoryRepo.Object);

// Test business logic without database dependencies
```

This example demonstrates how the Repository pattern provides a clean separation between data access and business logic, making your application more maintainable, testable, and flexible.