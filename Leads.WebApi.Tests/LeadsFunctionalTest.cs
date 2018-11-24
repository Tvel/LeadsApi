namespace Leads.WebApi.Tests
{
    using System;
    using System.Net;
    using System.Text;
    using System.Net.Http;
    using System.Threading.Tasks;
    using Newtonsoft.Json;
    using Xunit;
    using Leads.Models;
    using Leads.WebApi.Models;

    public class LeadsFunctionalTest  : IClassFixture<TestingConfigurationFactory<Startup>>
    {
        private readonly HttpClient client;

        public LeadsFunctionalTest(TestingConfigurationFactory<Startup> factory)
        {
            client = factory.CreateClient();
        }

        private string GetLeadSaveModelJson(
            string name,
            string address,
            string pincode,
            int? subAreaId,
            string mobileNumber,
            string email)
        {
            return JsonConvert.SerializeObject(new LeadsSaveViewModel
                                                   {
                                                       Address = address,
                                                       Name = name,
                                                       PinCode = pincode,
                                                       SubAreaId = subAreaId,
                                                       MobileNumber = mobileNumber,
                                                       Email = email
                                                   });
        }

        private async Task<string> CallSaveLeadApi(
            string name,
            string address,
            string pincode,
            int? subAreaId,
            string mobileNumber,
            string email)
        {
            var leadSaveModelJson = GetLeadSaveModelJson(name, address, pincode, subAreaId,mobileNumber, email);
            var response = await client.PostAsync("api/leads", new StringContent(leadSaveModelJson, Encoding.UTF8, "application/json"));
            response.EnsureSuccessStatusCode();
            return await response.Content.ReadAsStringAsync();
        }

        [Fact]
        public async void SavingCorrectLead_ReturnsId()
        {
            var content = await CallSaveLeadApi("name", "addr", "123", 1, "123456", "email@email.email");

            Assert.False(string.IsNullOrWhiteSpace(content));

            var leadSaveReturnModel = JsonConvert.DeserializeObject<LeadsSaveSuccessModel>(content);

            Assert.NotNull(leadSaveReturnModel);
            Assert.NotEqual(Guid.Empty, leadSaveReturnModel.Id);
        }

        [Fact]
        public async void SavingWithoutName_ReturnsError()
        {
            var leadSaveModelJson = GetLeadSaveModelJson(null, "addr", "123", 1, "123456", "email@email.email");
            var response = await client.PostAsync("api/leads", new StringContent(leadSaveModelJson, Encoding.UTF8, "application/json"));
            var content = await response.Content.ReadAsStringAsync();

            Assert.Equal(HttpStatusCode.BadRequest, response.StatusCode);
            Assert.False(string.IsNullOrWhiteSpace(content));

            var leadSaveReturnModel = JsonConvert.DeserializeObject<ErrorViewModel>(content);

            Assert.NotNull(leadSaveReturnModel);
            Assert.NotNull(leadSaveReturnModel.Message);
            Assert.Contains("name", leadSaveReturnModel.Message.ToLower());
        }

        [Fact]
        public async void SavingWithoutAddress_ReturnsError()
        {
            var leadSaveModelJson = GetLeadSaveModelJson("name", null, "123", 1, "123456", "email@email.email");
            var response = await client.PostAsync("api/leads", new StringContent(leadSaveModelJson, Encoding.UTF8, "application/json"));
            var content = await response.Content.ReadAsStringAsync();

            Assert.Equal(HttpStatusCode.BadRequest, response.StatusCode);
            Assert.False(string.IsNullOrWhiteSpace(content));

            var leadSaveReturnModel = JsonConvert.DeserializeObject<ErrorViewModel>(content);

            Assert.NotNull(leadSaveReturnModel);
            Assert.NotNull(leadSaveReturnModel.Message);
            Assert.Contains("address", leadSaveReturnModel.Message.ToLower());
        }

        [Fact]
        public async void SavingWithoutPinCode_ReturnsError()
        {
            var leadSaveModelJson = GetLeadSaveModelJson("name", "addr", null, 1, "123456", "email@email.email");
            var response = await client.PostAsync("api/leads", new StringContent(leadSaveModelJson, Encoding.UTF8, "application/json"));
            var content = await response.Content.ReadAsStringAsync();

            Assert.Equal(HttpStatusCode.BadRequest, response.StatusCode);
            Assert.False(string.IsNullOrWhiteSpace(content));

            var leadSaveReturnModel = JsonConvert.DeserializeObject<ErrorViewModel>(content);

            Assert.NotNull(leadSaveReturnModel);
            Assert.NotNull(leadSaveReturnModel.Message);
            Assert.Contains("pincode", leadSaveReturnModel.Message.ToLower());
        }

        [Fact]
        public async void SavingWithoutSubAreaId_ReturnsError()
        {
            var leadSaveModelJson = GetLeadSaveModelJson("name", "addr", "123", null, "123456", "email@email.email");
            var response = await client.PostAsync("api/leads", new StringContent(leadSaveModelJson, Encoding.UTF8, "application/json"));
            var content = await response.Content.ReadAsStringAsync();

            Assert.Equal(HttpStatusCode.BadRequest, response.StatusCode);
            Assert.False(string.IsNullOrWhiteSpace(content));

            var leadSaveReturnModel = JsonConvert.DeserializeObject<ErrorViewModel>(content);

            Assert.NotNull(leadSaveReturnModel);
            Assert.NotNull(leadSaveReturnModel.Message);
            Assert.Contains("subarea", leadSaveReturnModel.Message.ToLower());
        }

        [Fact]
        public async void SavingWithoutMatchingSubAreaAndPinCode_ReturnsError()
        {
            var leadSaveModelJson = GetLeadSaveModelJson("name", "addr", "456", 1, "123456", "email@email.email");
            var response = await client.PostAsync("api/leads", new StringContent(leadSaveModelJson, Encoding.UTF8, "application/json"));
            var content = await response.Content.ReadAsStringAsync();

            Assert.Equal(HttpStatusCode.BadRequest, response.StatusCode);
            Assert.False(string.IsNullOrWhiteSpace(content));

            var leadSaveReturnModel = JsonConvert.DeserializeObject<ErrorViewModel>(content);

            Assert.NotNull(leadSaveReturnModel);
            Assert.NotNull(leadSaveReturnModel.Message);
            Assert.Contains("subarea", leadSaveReturnModel.Message.ToLower());
        }

        [Fact]
        public async void GetByExistingId_ReturnsLead()
        {
            var idJson = await CallSaveLeadApi("name", "addr", "123", 1, "123456", "email@email.email");
            var leadSaveReturnModel = JsonConvert.DeserializeObject<LeadsSaveSuccessModel>(idJson);

            var response = await client.GetAsync($"api/leads/{leadSaveReturnModel.Id}");
            response.EnsureSuccessStatusCode();
            var content = await response.Content.ReadAsStringAsync();

            Assert.False(string.IsNullOrWhiteSpace(content));
            var lead = JsonConvert.DeserializeObject<LeadViewModel>(content);
            Assert.Equal("name", lead.Name);
            Assert.Equal("addr", lead.Address);
            Assert.Equal("123", lead.PinCode);
        }

        [Fact]
        public async void GetByInvalidGuid_Returns400()
        {
            var response = await client.GetAsync($"api/leads/asd-asd");
            Assert.Equal(HttpStatusCode.BadRequest, response.StatusCode);
        }

        [Fact]
        public async void GetByNonExistingId_Returns404()
        {
            var response = await client.GetAsync($"api/leads/a8664afa-bc9e-4cfd-9e62-d9d1349c46f1");
            Assert.Equal(HttpStatusCode.NotFound, response.StatusCode);
        }
    }
}
