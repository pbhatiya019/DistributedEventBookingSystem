using DistributedEventBookingSystem.Tests.Helpers;
using TicketService.DTOs;
using Xunit;

namespace DistributedEventBookingSystem.Tests.TicketServiceTests
{
    public class CreateTicketDtoTests
    {
        [Fact]
        public void CreateTicketDto_ShouldBeValid_WhenDataIsCorrect()
        {
            // Arrange
            var dto = new CreateTicketDto
            {
                EventId = 1,
                AttendeeName = "Pratham Bhatiya",
                AttendeeEmail = "pratham@example.com"
            };

            // Act
            var results = ValidationHelper.ValidateModel(dto);

            // Assert
            Assert.Empty(results);
        }

        [Fact]
        public void CreateTicketDto_ShouldBeInvalid_WhenEmailIsIncorrect()
        {
            // Arrange
            var dto = new CreateTicketDto
            {
                EventId = 1,
                AttendeeName = "Pratham Bhatiya",
                AttendeeEmail = "not-an-email"
            };

            // Act
            var results = ValidationHelper.ValidateModel(dto);

            // Assert
            Assert.NotEmpty(results);
            Assert.Contains(results, r => r.MemberNames.Contains("AttendeeEmail"));
        }
    }
}