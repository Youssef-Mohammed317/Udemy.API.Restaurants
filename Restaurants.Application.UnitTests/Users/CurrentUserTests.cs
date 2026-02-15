using Moq;
using Restaurants.Application;
using Restaurants.Application.Users;
using Restaurants.Domain.Constants;
using System;
using System.Collections.Generic;
using System.Linq;
using Xunit;


namespace Restaurants.Application.Users.UnitTests;

public class CurrentUserTests
{
    [Theory()]
    [InlineData(UserRoles.Admin)]
    [InlineData(UserRoles.User)]
    public void IsInRole_TestAdmin_ReturnsTrue(string roleName)
    {
        // Arrange
        var user = new CurrentUser(Id: "user-id", Email: "test@test.com", Roles: [UserRoles.Admin,UserRoles.User], Nationality: null, DateOfBirth: null);

        // Act
        var actual = user.IsInRole(roleName);

        // Assert
        Assert.True(actual);
    }
    
    [Fact]
    public void IsInRole_TestOwner_ReturnsFalse()
    {
        // Arrange
        var user = new CurrentUser(Id: "user-id", Email: "test@test.com", Roles: [UserRoles.Admin,UserRoles.User], Nationality: null, DateOfBirth: null);

        // Act
        var actual = user.IsInRole(UserRoles.Owner);

        // Assert
        Assert.False(actual);
    }
}