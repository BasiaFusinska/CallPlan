using System;
using System.Threading.Tasks;
using CallPlan;
using FluentAssertions;
using Moq;
using NUnit.Framework;

namespace CallPlanTests
{
    [TestFixture]
    public class ServiceHandlerTests
    {
        [Test]
        public async void proper_number_from_service_should_return_proper_service_response()
        {
            var categoryServiceMock = new Mock<ICategoryService>();
            categoryServiceMock.Setup(s => s.GetResponse(It.IsAny<string>())).Returns(Task.FromResult("1"));

            var serviceHandler = new ServiceHandler(categoryServiceMock.Object);
            var response = await serviceHandler.HandleService("originator");

            response.Should().Be(ServiceResponse.Response1);
        }

        [Test]
        [ExpectedException(typeof(FormatException))]
        public async void invalid_test_from_service_should_throw_exception()
        {
            var categoryServiceMock = new Mock<ICategoryService>();
            categoryServiceMock.Setup(s => s.GetResponse(It.IsAny<string>())).Returns(Task.FromResult("xxx"));

            var serviceHandler = new ServiceHandler(categoryServiceMock.Object);
            await serviceHandler.HandleService("originator");
        }

        [Test]
        public async void timeout_from_service_should_return_timeout_service_response()
        {
            var categoryServiceMock = new Mock<ICategoryService>();
            categoryServiceMock.Setup(s => s.GetResponse(It.IsAny<string>())).Callback<string>(
                _ => { throw new TimeoutException(); });

            var serviceHandler = new ServiceHandler(categoryServiceMock.Object);
            var response = await serviceHandler.HandleService("originator");

            response.Should().Be(ServiceResponse.Timeout);
        }

        [Test]
        public async void general_exception_from_service_should_return_exception_service_response()
        {
            var categoryServiceMock = new Mock<ICategoryService>();
            categoryServiceMock.Setup(s => s.GetResponse(It.IsAny<string>())).Callback<string>(
                _ => { throw new Exception(); });

            var serviceHandler = new ServiceHandler(categoryServiceMock.Object);
            var response = await serviceHandler.HandleService("originator");

            response.Should().Be(ServiceResponse.Exception);
        }
    }
}
