using Xunit;
using Restaurants.API.Middlewares;
using System;
using System.Collections.Generic;
using System.Text;
using Microsoft.Extensions.Logging;
using Moq;
using Microsoft.AspNetCore.Http;
using System.Threading.Tasks;
using Restaurants.Domain.Exceptions;
using Restaurants.Domain.Entities;
using Assert = Xunit.Assert;

namespace Restaurants.API.Middlewares.Tests
{
    public class ErrorHandlingMiddlewareTests
    {
        private Mock<ILogger<ErrorHandlingMiddleware>> loggerMock;
        private DefaultHttpContext httpContext;

        private Mock<HttpContext> httpContextMock;
        private Mock<RequestDelegate> nextDel;

        public ErrorHandlingMiddlewareTests()
        {
            loggerMock = new Mock<ILogger<ErrorHandlingMiddleware>>();
            httpContextMock = new Mock<HttpContext>();
            httpContext = new DefaultHttpContext();
            nextDel = new Mock<RequestDelegate>();
        }
        [Fact()]
        public async Task InvokeAsync_WhenNoExceptionThrown_ShouldCallNextDelegate()
        {
            var middleware = new ErrorHandlingMiddleware(loggerMock.Object);

            // act
            //await middleware.InvokeAsync(httpContextMock.Object, nextDel.Object);
            await middleware.InvokeAsync(httpContext, nextDel.Object);

            // assert
            nextDel.Verify(next => next(It.IsAny<HttpContext>()), Times.Once);

        }
        [Fact()]
        public async Task InvokeAsync_WhenNotFoundExceptionThrown_ShouldRetrun404()
        {
            // Arrange
            var middleware = new ErrorHandlingMiddleware(loggerMock.Object);
            var notFoundException = new NotFoundException(nameof(Restaurant), "1");

            // Act
            await middleware.InvokeAsync(httpContext, _ => throw notFoundException);

            // assert

            Assert.Equal(StatusCodes.Status404NotFound, httpContext.Response.StatusCode);
        }
        [Fact()]
        public async Task InvokeAsync_WhenForbiddenExceptionThrown_ShouldRetrun403()
        {
            // Arrange
            var middleware = new ErrorHandlingMiddleware(loggerMock.Object);
            var forbiddenException = new ForbiddenException(nameof(Restaurant), "1", "Create");

            // Act
            await middleware.InvokeAsync(httpContext, _ => throw forbiddenException);

            // assert

            Assert.Equal(StatusCodes.Status403Forbidden, httpContext.Response.StatusCode);
        }
    }
}