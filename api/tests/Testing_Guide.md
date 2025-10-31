### Test-writing guidelines for layered .NET backends (unit-first, framework-agnostic)

This guide describes broadly applicable patterns for writing unit tests in layered .NET applications that use patterns like CQRS/mediators, HTTP endpoints (minimal APIs, controllers, or endpoint libraries), and repository-based persistence (EF Core or otherwise). Examples are illustrative and can be adapted to your stack.

## Core principles

- **Unit-first**: Test behavior of a single unit (handler, service, endpoint, repository) in isolation with minimal collaborators.
- **Deterministic**: No randomness, time sensitivity, or network/system dependencies. Prefer pure inputs/outputs.
- **Meaningful names**: Use `Subject_Scenario_ExpectedResult` naming, e.g. `CreateUser_WhenNameMissing_ReturnsValidationError`.
- **Fast and independent**: Each test should be self-contained and runnable in any order.

## Recommended tooling (pick equivalents as needed)

- **Test framework**: NUnit, xUnit, or MSTest. Choose one and stay consistent.
- **Mocking**: Moq, NSubstitute, or JustMock for substituting interfaces and collaborators.
- **Object mapping**: If using AutoMapper, configure real profiles in tests where mapping is part of behavior.
- **Persistence**: EF Core InMemory for repository tests, or pure in-memory fakes for custom data layers.
- **Coverage**: Coverlet or your CI’s built-in tooling.

Example .csproj snippet (adjust versions/frameworks as needed):

```xml
<ItemGroup>
  <PackageReference Include="Microsoft.NET.Test.Sdk" Version="17.*" />
  <PackageReference Include="NUnit" Version="4.*" />
  <PackageReference Include="NUnit3TestAdapter" Version="4.*" />
  <PackageReference Include="Moq" Version="4.*" />
  <PackageReference Include="coverlet.collector" Version="6.*" />
</ItemGroup>
```

## Project layout for tests

Organize by responsibility, not by technology names of a specific project:

- `ApplicationTests/` or `DomainTests/`: Business logic units like command/query handlers or services. Collaborators (repositories, external services) are mocked.
- `EndpointTests/` or `ApiTests/`: HTTP-facing units (controllers, minimal API handlers, endpoint classes). Auth/token and mediator/service dependencies are mocked.
- `PersistenceTests/`: Repository or data access units. Use in-memory database or fakes; verify behavior and side effects.
- Optional: `InfrastructureTests/` for email, payments, and other adapters using mocks/fakes.

Add `GlobalUsings.cs` with your chosen test framework to reduce ceremony:

```csharp
global using NUnit.Framework;
```

## Conventions inside test classes

- Use `[SetUp]`/`[TearDown]` (NUnit) or constructor/`IDisposable` for per-test setup/cleanup.
- Prefer `Assert.Multiple` (NUnit) or grouped assertions to keep failures readable.
- Inject `ILogger<T>` with `Mock.Of<ILogger<T>>()` or test logger fakes when logging is not under test.
- Keep data builders/simple factory methods near the tests to avoid repetition.

## Testing application/business logic (handlers/services)

Goal: verify the unit’s decision-making given inputs and collaborator outcomes.

Pattern:
- Mock collaborators (repositories, mediator/bus, clock, id generator).
- If mapping is integral, initialize the real mapping profiles for realistic conversions.
- Assert on returned DTOs/flags/messages and on expected calls to collaborators.

Example:

```csharp
[TestFixture]
public class CreateOrderHandlerTests
{
    private Mock<IOrderRepository> _orders = null!;
    private Mock<INotifier> _notifier = null!;
    private CreateOrderHandler _handler = null!;

    [SetUp]
    public void Setup()
    {
        _orders = new Mock<IOrderRepository>();
        _notifier = new Mock<INotifier>();
        _handler = new CreateOrderHandler(_orders.Object, _notifier.Object);
    }

    [Test]
    public async Task CreateOrder_WhenValid_ReturnsSuccess()
    {
        var cmd = new CreateOrderCommand { CustomerId = 1, Items = new[] { new OrderItemDto { ProductId = 10, Quantity = 2 } } };
        _orders.Setup(r => r.AddAsync(It.IsAny<Order>())).ReturnsAsync(123);

        var result = await _handler.Handle(cmd, CancellationToken.None);

        Assert.Multiple(() =>
        {
            Assert.That(result.Success, Is.True);
            Assert.That(result.OrderId, Is.EqualTo(123));
        });
        _notifier.Verify(n => n.SendOrderCreated(123), Times.Once);
    }
}
```

Coverage considerations:
- Happy path, validation failures, collaborator failures, and edge cases (nulls, empties, limits, uniqueness).

## Testing HTTP endpoints (controllers/minimal APIs/endpoint classes)

Goal: verify request handling, auth/validation branching, and interaction with application layer.

Pattern:
- Mock auth/token validators and application services/mediators.
- Instantiate endpoint/controller directly; avoid full web server unless doing integration tests.
- Call handler method, then assert on the response model and status behavior.

Example:

```csharp
[TestFixture]
public class CreateProductEndpointTests
{
    private Mock<ITokenValidator> _tokens = null!;
    private Mock<IApplicationBus> _bus = null!;
    private CreateProductEndpoint _endpoint = null!;

    [SetUp]
    public void Setup()
    {
        _tokens = new Mock<ITokenValidator>();
        _bus = new Mock<IApplicationBus>();
        _endpoint = new CreateProductEndpoint(_bus.Object, _tokens.Object);
    }

    [Test]
    public async Task Handle_WithValidToken_ReturnsSuccess()
    {
        var request = new CreateProductRequest { /* ... */ };
        _tokens.Setup(t => t.ValidateAsync(It.IsAny<string>())).ReturnsAsync(true);
        _bus.Setup(b => b.Send(It.IsAny<CreateProductCommand>(), It.IsAny<CancellationToken>()))
            .ReturnsAsync(new CreateProductResult { Success = true });

        await _endpoint.HandleAsync(request, CancellationToken.None);
        var response = _endpoint.Response; // Or returned IActionResult/DTO

        Assert.That(response.Success, Is.True);
    }
}
```

Coverage considerations:
- Authorized vs unauthorized, validation errors, exception path producing generic error responses, and correct mapping of application results to API responses.

## Testing persistence (repositories/data access)

Goal: verify repository behavior, queries, and side effects without external infrastructure.

Options:
- EF Core: use `UseInMemoryDatabase` for quick feedback; seed deterministic data in setup and clean in teardown.
- Custom data layer: implement simple in-memory fakes with predictable behavior for unit tests; reserve integration tests for real databases.

Example with EF Core:

```csharp
[TestFixture]
public class ProductRepositoryTests
{
    private AppDbContext _db = null!;

    [SetUp]
    public void Setup()
    {
        var options = new DbContextOptionsBuilder<AppDbContext>()
            .UseInMemoryDatabase("TestDb")
            .Options;
        _db = new AppDbContext(options);
        _db.Products.AddRange(new Product { Id = 1 }, new Product { Id = 2 });
        _db.SaveChanges();
    }

    [TearDown]
    public void TearDown()
    {
        _db.Database.EnsureDeleted();
        _db.Dispose();
    }

    [Test]
    public async Task GetById_WhenExists_ReturnsEntity()
    {
        var repo = new ProductRepository(_db);
        var result = await repo.GetByIdAsync(1);
        Assert.That(result, Is.Not.Null);
    }
}
```

Coverage considerations:
- Exists vs not exists, add/update/delete success and failure paths, query filtering, and concurrency/transactional concerns where applicable.

## Shared tips

- Centralize fixture creation with builders to avoid duplication and improve readability.
- Prefer verifying observable outcomes over internal implementation details.
- Keep mocks strict only when helpful; over-specifying interactions makes tests brittle.
- Use domain constants/messages where they are part of the contract; otherwise assert shapes/flags.
- Group related assertions to reduce noisy failures.

## Running tests and coverage

```bash
dotnet test
dotnet test --collect:"XPlat Code Coverage"
```

Integrate with CI to publish coverage and test results. Keep unit tests fast; push end-to-end scenarios to separate integration test projects.


