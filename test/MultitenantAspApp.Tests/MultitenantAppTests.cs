using System;
using System.Linq;
using System.Threading.Tasks;
using Xunit;

namespace MultitenantAspApp.Tests
{

    public class HomeControllerIndexShould : BaseWebTest
    {
        [Fact]
        public async Task SpecificTenant_IsCorrectlyIdentified()
        {
            var response = await _client.GetAsync("/SZ/api/ValuesWithDependencies");
            response.EnsureSuccessStatusCode();
            var stringResponse = await response.Content.ReadAsStringAsync();

            Assert.Contains("SZ: Hello World", stringResponse);
        }

        [Fact]
        public async Task GenericTenant_UsesDefaultDependencies()
        {
            var response = await _client.GetAsync("/xxx/api/ValuesWithDependencies");
            response.EnsureSuccessStatusCode();
            var stringResponse = await response.Content.ReadAsStringAsync();
            
            Assert.Contains("Hello World", stringResponse);
        }

        [Fact]
        public async Task GenericTenant_IsCorrectlyPassedToActionMethod()
        {
            // we just want a guid with no numbers in it
            // (https://stackoverflow.com/questions/18645672/how-do-i-create-a-unique-identifier-that-does-not-contain-numbers)
            var tenantId = String.Concat(Guid.NewGuid().ToString("N").Select(c => (char)(c + 17)));
            var response = await _client.GetAsync($"/{tenantId}/api/ValuesWithDependencies");
            response.EnsureSuccessStatusCode();
            var stringResponse = await response.Content.ReadAsStringAsync();

            Assert.Contains($"{tenantId}", stringResponse);
        }
    }
}