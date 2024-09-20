using Clean_Architecture.Application.Common.Behaviours;
using Clean_Architecture.Application.Common.Interfaces;
using Clean_Architecture.Application.TodoItems.Commands.CreateTodoItem;
using Microsoft.Extensions.Logging;
using Moq;
using NUnit.Framework;

namespace Clean_Architecture.Application.UnitTests.Common.Behaviours;

public class RequestLoggerTests
{
    private Mock<ILogger<CreateTodoItemCommand>> _logger = null!;
    private Mock<IUser> _user = null!;
    private Mock<IIdentityService> _identityService = null!;

    [SetUp]
    public void Setup()
    {
        _logger = new Mock<ILogger<CreateTodoItemCommand>>();
        _user = new Mock<IUser>();
        _identityService = new Mock<IIdentityService>();
    }

    [Test]
    public async Task ShouldCallGetUserNameAsyncOnceIfAuthenticated()
    {
        _user.Setup(x => x.Id).Returns(Guid.NewGuid().ToString());
        _identityService.Setup(x => x.GetUserNameAsync(It.IsAny<string>())).ReturnsAsync("Test User");

        var requestLogger = new LoggingBehaviour<CreateTodoItemCommand, int>(_logger.Object, _user.Object, _identityService.Object);

        await requestLogger.Handle(new CreateTodoItemCommand { ListId = 1, Title = "title" }, () => Task.FromResult(1), new CancellationToken());

        _identityService.Verify(i => i.GetUserNameAsync(It.IsAny<string>()), Times.Once);
    }

    [Test]
    public async Task ShouldNotCallGetUserNameAsyncIfUnauthenticated()
    {
        var requestLogger = new LoggingBehaviour<CreateTodoItemCommand, int>(_logger.Object, _user.Object, _identityService.Object);

        await requestLogger.Handle(new CreateTodoItemCommand { ListId = 1, Title = "title" }, () => Task.FromResult(1), new CancellationToken());

        _identityService.Verify(i => i.GetUserNameAsync(It.IsAny<string>()), Times.Never);
    }
}
