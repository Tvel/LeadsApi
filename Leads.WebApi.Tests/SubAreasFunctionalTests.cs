namespace Leads.WebApi.Tests
{
    using System.Collections.Generic;
    using System.Linq;
    using System.Net.Http;
    using Newtonsoft.Json;
    using Xunit;

    using Leads.Models;

    public class SubAreasFunctionalTests : IClassFixture<TestingConfigurationFactory<Startup>>
    {
        private readonly HttpClient client;

        public SubAreasFunctionalTests(TestingConfigurationFactory<Startup> factory)
        {
            client = factory.CreateClient();
        }

        [Fact]
        public async void GetAll_ReturnsContent()
        {
            var response = await client.GetAsync("api/subareas");
            response.EnsureSuccessStatusCode();
            var content = await response.Content.ReadAsStringAsync();

            Assert.False(string.IsNullOrWhiteSpace(content));
        }

        [Fact]
        public async void GetByPin_ReturnsFilteredContent()
        {
            var response = await client.GetAsync("api/subareas/filter/pincode/567");
            response.EnsureSuccessStatusCode();
            var content = await response.Content.ReadAsStringAsync();

            Assert.False(string.IsNullOrWhiteSpace(content));

            var subareas = JsonConvert.DeserializeObject<List<SubAreaViewModel>>(content);
            Assert.True(subareas.Select(s => s.PinCode).All(p => p == "567"));
        }

        [Fact]
        public async void GetByNonExistingPin_ReturnsEmptyContent()
        {
            var response = await client.GetAsync("api/subareas/filter/pincode/999");
            response.EnsureSuccessStatusCode();
            var content = await response.Content.ReadAsStringAsync();

            Assert.False(string.IsNullOrWhiteSpace(content));

            var subareas = JsonConvert.DeserializeObject<List<SubAreaViewModel>>(content);
            Assert.Empty(subareas);
        }
    }
}
