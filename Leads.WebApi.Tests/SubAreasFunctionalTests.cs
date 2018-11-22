﻿namespace Leads.WebApi.Tests
{
    using System;
    using System.Collections.Generic;
    using System.Text;
    using System.Net.Http;

    using Microsoft.AspNetCore.Mvc.Testing;

    using Xunit;

    public class SubAreasFunctionalTests : IClassFixture<TestingConfigurationFactory<Startup>>
    {
        protected readonly HttpClient _client;

        public SubAreasFunctionalTests(TestingConfigurationFactory<Startup> factory)
        {
            _client = factory.CreateClient();

        }

        // write tests that use _client

        [Fact]
        public async void GetAll_ReturnsContent()
        {
            var response = await _client.GetAsync("api/subareas");
            response.EnsureSuccessStatusCode();
            var content = await response.Content.ReadAsStringAsync();

            Assert.False(string.IsNullOrWhiteSpace(content));
        }
    }
}