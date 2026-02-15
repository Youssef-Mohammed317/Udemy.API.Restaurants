using Microsoft.AspNetCore.Http;
using Moq;
using System.Security.Claims;

namespace Restaurants.Application.Users.UnitTests;

public class UserContextTests
{
    [Fact]
    public void GetCurrentUser_UserAuthenticated_ReturnsNotNullUser()
    {
        // Arrange
        var claims = new List<Claim>
        {
            new Claim(ClaimTypes.NameIdentifier, "user-123"),
            new Claim(ClaimTypes.Email, "test@example.com"),
            new Claim(ClaimTypes.Role, "Admin"),
            new Claim(ClaimTypes.Role, "User"),
        };

        var userPrincipal = new ClaimsPrincipal(new ClaimsIdentity(claims, authenticationType: "TestAuth"));
        var httpContext = new DefaultHttpContext { User = userPrincipal };

        var accessorMock = new Mock<IHttpContextAccessor>();
        accessorMock.Setup(a => a.HttpContext).Returns(httpContext);

        var userContext = new UserContext(accessorMock.Object);

        // Act
        var currentUser = userContext.GetCurrentUser();

        // Assert
        Assert.NotNull(currentUser);
        Assert.Equal("user-123", currentUser!.Id);
        Assert.Equal("test@example.com", currentUser.Email);
        Assert.Contains("Admin", currentUser.Roles);
        Assert.Contains("User", currentUser.Roles);
    }

    [Fact]
    public void GetCurrentUser_UserNotAuthenticated_ReturnsNull()
    {
        // Arrange
        var accessorMock = new Mock<IHttpContextAccessor>();
        accessorMock.Setup(a => a.HttpContext).Returns((HttpContext)null);
        var userContext = new UserContext(accessorMock.Object);
        // Act
        var action = () => userContext.GetCurrentUser();
        // Assert
        Assert.Throws<InvalidOperationException>(action);
    }
}
