using System;
using System.Collections.Generic;
using Xunit;
using Leads.Services.Tests.Mocks;
using Leads.Models;

namespace Leads.Services.Tests
{
    public class SubAreasTest
    {
        private readonly SubAreasMock subAreasDbMock;

        private readonly SubAreasService subAreasService;

        public SubAreasTest()
        {
            this.subAreasDbMock = new SubAreasMock();

            this.subAreasService = new SubAreasService(this.subAreasDbMock);
        }

        [Fact]
        public void SubAreasCanBeConstructed()
        {
            Assert.NotNull(this.subAreasService);
        }

        [Fact]
        public async void GetAll_ReturnsResultFromDb()
        {
            var resultList = new List<SubAreaViewModel>();
            this.subAreasDbMock.GetAllReturn = resultList;

            var result = await this.subAreasService.GetAll().ConfigureAwait(false);
            Assert.Equal(resultList, result);
        }

        [Fact]
        public async void GetAll_CallsDb()
        {
            var result = await this.subAreasService.GetAll().ConfigureAwait(false);
            Assert.True(this.subAreasDbMock.IsGetAllCalled);
        }

        [Fact]
        public async void GetByPinCode_ReturnsResultFromDb()
        {
            var resultList = new List<SubAreaViewModel>();
            this.subAreasDbMock.GetByPinCodeReturn = resultList;

            var result = await this.subAreasService.GetByPinCode("123").ConfigureAwait(false);
            Assert.Equal(resultList, result);
        }

        [Fact]
        public async void GetByPinCode_CallsDb()
        {
            var resultList = new List<SubAreaViewModel>();

            var result = await this.subAreasService.GetByPinCode("123").ConfigureAwait(false);
            Assert.True(this.subAreasDbMock.IsGetByPinCodeCalled);
        }

        [Fact]
        public async void GetByPinCode_ThrowsIfNull()
        {
            var resultList = new List<SubAreaViewModel>();

            await Assert.ThrowsAsync<ArgumentException>(
                async () => await this.subAreasService.GetByPinCode(null).ConfigureAwait(false));
        }
    }
}
