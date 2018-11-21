using System;
using System.Collections.Generic;
using System.Text;
using Xunit;
using Leads.Services.Tests.Mocks;
using Leads.Models;

namespace Leads.Services.Tests
{
    public class SubAreasTest
    {
        private readonly SubAreasMock subAreasDbMock;

        private readonly SubAreas subAreas;

        public SubAreasTest()
        {
            this.subAreasDbMock = new SubAreasMock();

            this.subAreas = new SubAreas(this.subAreasDbMock);
        }

        [Fact]
        public void SubAreasCanBeConstructed()
        {
            Assert.NotNull(this.subAreas);
        }

        [Fact]
        public async void GetAllReturnsResultFromDb()
        {
            var resultList = new List<SubAreaViewModel>();
            this.subAreasDbMock.GetAllReturn = resultList;

            var result = await this.subAreas.GetAll().ConfigureAwait(false);
            Assert.Equal(resultList, result);
        }

        [Fact]
        public async void GetAllCallsDb()
        {
            var result = await this.subAreas.GetAll().ConfigureAwait(false);
            Assert.True(this.subAreasDbMock.IsGetAllCalled);
        }

        [Fact]
        public async void GetByPinCodeReturnsResultFromDb()
        {
            var resultList = new List<SubAreaViewModel>();
            this.subAreasDbMock.GetByPinCodeReturn = resultList;

            var result = await this.subAreas.GetByPinCode("123").ConfigureAwait(false);
            Assert.Equal(resultList, result);
        }

        [Fact]
        public async void GetByPinCodeCallsDb()
        {
            var resultList = new List<SubAreaViewModel>();

            var result = await this.subAreas.GetByPinCode("123").ConfigureAwait(false);
            Assert.True(this.subAreasDbMock.IsGetByPinCodeCalled);
        }

        [Fact]
        public async void GetByPinCodeThrowsIfNull()
        {
            var resultList = new List<SubAreaViewModel>();

            await Assert.ThrowsAsync<ArgumentException>(
                async () => await this.subAreas.GetByPinCode(null).ConfigureAwait(false));
        }
    }
}
