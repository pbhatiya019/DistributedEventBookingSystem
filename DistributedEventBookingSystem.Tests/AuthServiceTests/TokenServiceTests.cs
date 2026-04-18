using System.IdentityModel.Tokens.Jwt;
using AuthService.Models;
using AuthService.Services;
using Microsoft.Extensions.Configuration;
using Xunit;

namespace DistributedEventBookingSystem.Tests.AuthServiceTests
{
    public class TokenServiceTests
    {
        private readonly TokenService _tokenService;

        public TokenServiceTests()
        {
            var inMemorySettings = new Dictionary<string, string?>
            {
                {"Jwt:Key", "ThisIsASecretKeyForDistributedEventBookingSystem123!"},
                {"Jwt:Issuer", "AuthService"},
                {"Jwt:Audience", "DistributedEventBookingSystemUsers"}
            };

            IConfiguration configuration = new ConfigurationBuilder()
                .AddInMemoryCollection(inMemorySettings)
                .Build();

            _tokenService = new TokenService(configuration);
        }

        [Fact]
        public void CreateToken_ShouldReturn_NonEmptyToken()
        {
            // Arrange
            var user = new User
            {
                Id = 1,
                FullName = "Pratham Bhatiya",
                Email = "pratham@example.com",
                PasswordHash = "hashed-password"
            };

            // Act
            var token = _tokenService.CreateToken(user);

            // Assert
            Assert.False(string.IsNullOrWhiteSpace(token));
        }

        [Fact]
        public void CreateToken_ShouldContain_EmailClaim()
        {
            // Arrange
            var user = new User
            {
                Id = 2,
                FullName = "Pratham Bhatiya",
                Email = "pratham@example.com",
                PasswordHash = "hashed-password"
            };

            // Act
            var token = _tokenService.CreateToken(user);

            var handler = new JwtSecurityTokenHandler();
            var jwtToken = handler.ReadJwtToken(token);

            // Assert
            Assert.Contains(jwtToken.Claims, c => c.Type == "email" && c.Value == "pratham@example.com");
        }
    }
}