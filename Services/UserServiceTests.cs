using Microsoft.AspNetCore.Mvc.Testing;
using Moq;
using SafeVaultApp.DbModels;
using Xunit;

namespace SafeVaultApp.Services
{
    public class UserServiceTests : IClassFixture<WebApplicationFactory<Program>>
    {
        private readonly WebApplicationFactory<Program> _factory;

        public UserServiceTests(WebApplicationFactory<Program> factory)
        {
            _factory = factory;
        }

        [Fact]
        public void ShouldNotAuthenticateWithSqlInjection()
        {
            // Mock IConfiguration  
            var mockConfig = new Mock<IConfiguration>();

            // Mock AppDbContext (replace with your actual DbContext mock setup)  
            var mockContext = new Mock<AppDbContext>();

            // Pass mocks to AuthService constructor  
            var service = new AuthService(mockContext.Object, mockConfig.Object);

            var result = service.Authenticate("password" ,"' OR 1=1 --");

            Assert.Null(result); // Should not authenticate  
        }
        [Fact]
        public async Task ShouldSanitizeXssPayload()
        {
            var client = _factory.CreateClient();
            var payload = new { username = "<script>alert('xss')</script>" };

            var response = await client.PostAsJsonAsync("/api/auth/login", payload);
            var content = await response.Content.ReadAsStringAsync();

            Assert.DoesNotContain("<script>", content);
        }

    }

}
