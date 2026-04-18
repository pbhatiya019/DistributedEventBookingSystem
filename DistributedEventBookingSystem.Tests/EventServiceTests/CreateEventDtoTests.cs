using DistributedEventBookingSystem.Tests.Helpers;
using EventService.DTOs;
using Xunit;

namespace DistributedEventBookingSystem.Tests.EventServiceTests
{
    public class CreateEventDtoTests
    {
        [Fact]
        public void CreateEventDto_ShouldBeValid_WhenRequiredFieldsAreProvided()
        {
            // Arrange
            var dto = new CreateEventDto
            {
                Title = "Tech Event",
                Description = "Microservices final project event",
                Location = "Kitchener",
                EventDate = DateTime.UtcNow.AddDays(10),
                Capacity = 100
            };

            // Act
            var results = ValidationHelper.ValidateModel(dto);

            // Assert
            Assert.Empty(results);
        }

        [Fact]
        public void CreateEventDto_ShouldBeInvalid_WhenTitleIsMissing()
        {
            // Arrange
            var dto = new CreateEventDto
            {
                Title = "",
                Description = "Microservices final project event",
                Location = "Kitchener",
                EventDate = DateTime.UtcNow.AddDays(10),
                Capacity = 100
            };

            // Act
            var results = ValidationHelper.ValidateModel(dto);

            // Assert
            Assert.NotEmpty(results);
            Assert.Contains(results, r => r.MemberNames.Contains("Title"));
        }
    }
}