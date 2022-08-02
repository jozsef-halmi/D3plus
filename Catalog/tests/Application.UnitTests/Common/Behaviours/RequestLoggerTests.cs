using Catalog.Application.Categorys.Commands.CreateCategory;
using Catalog.Application.Common.Behaviours;
using Microsoft.Extensions.Logging;
using Moq;
using NUnit.Framework;

namespace Catalog.Application.UnitTests.Common.Behaviours;

public class RequestLoggerTests
{
    private Mock<ILogger<CreateCategoryCommand>> _logger = null!;

    [SetUp]
    public void Setup()
    {
        _logger = new Mock<ILogger<CreateCategoryCommand>>();
    }

    [Test]
    public async Task ShouldCallGetUserNameAsyncOnceIfAuthenticated()
    {
        var requestLogger = new LoggingBehaviour<CreateCategoryCommand>(_logger.Object);

        await requestLogger.Process(new CreateCategoryCommand { Name = "new category" }, new CancellationToken());
    }
}
