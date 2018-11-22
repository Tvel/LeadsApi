using System;
using Leads.Models;
using Leads.Services.Tests.Mocks;
using Xunit;

namespace Leads.Services.Tests
{
    public class LeadsTests
    {
        private readonly LeadsDbMock leadsMock;

        private readonly SubAreasMock subAreasMock;

        private readonly LeadsService leadsService;

        public LeadsTests()
        {
            this.leadsMock = new LeadsDbMock();
            this.subAreasMock = new SubAreasMock();

            this.leadsService = new LeadsService(this.leadsMock, this.subAreasMock);
        }

        [Fact]
        public void LeadsCanBeConstructed()
        {
            Assert.NotNull(this.leadsService);
        }

        [Fact]
        public async void WhenYouGetByIdDbReturnsCorrectViewModel()
        {
            var lead = new LeadViewModel();
            this.leadsMock.GetReturn = lead;

            var resultLead = await this.leadsService.Get(Guid.NewGuid()).ConfigureAwait(false);
            Assert.Equal(resultLead, lead);
            Assert.True(this.leadsMock.IsGetByIdCalled);
        }

        [Fact]
        public async void WhenYouGetByIdDbReturnsSubAreaAsWell()
        {
            var lead = new LeadViewModel();
            this.leadsMock.GetReturn = lead;
            var subArea = new SubAreaViewModel();
            this.subAreasMock.GetByIdReturn = subArea;

            var resultLead = await this.leadsService.Get(Guid.NewGuid()).ConfigureAwait(false);
            Assert.Equal(subArea, lead.SubArea);
            Assert.True(this.subAreasMock.IsGetByIdCalled);
        }

        [Fact]
        public async void WhenYouGetByInvalidIdDbReturnsNull()
        {
            leadsMock.GetReturn = null;

            var resultLead = await this.leadsService.Get(Guid.NewGuid()).ConfigureAwait(false);
            Assert.Null(resultLead);
        }

        [Fact]
        public async void WhenNullIsSentToSaveModelExceptionIsThrown()
        {
            await Assert.ThrowsAsync<ArgumentNullException>(
                async () => await this.leadsService.Save(null).ConfigureAwait(false));
        }

        [Fact]
        public async void WhenValidSaveModelIsSavedResultIsTrue()
        {
            subAreasMock.GetByIdReturn = new SubAreaViewModel{ PinCode = "123" };
            var saveModel = new LeadSaveModel
                                {
                Name = "name", Address = "addr", Email = "email@email.email", MobileNumber = "12345", PinCode = "123", SubAreaId = 1
                                };
            var viewModel = new LeadViewModel();
            leadsMock.SaveReturn = viewModel;

            var resultLead = await this.leadsService.Save(saveModel).ConfigureAwait(false);
            Assert.True(resultLead);
        }

        [Fact]
        public async void WhenValidSaveModelIsSavedDbIsCalled()
        {
            subAreasMock.GetByIdReturn = new SubAreaViewModel{ PinCode = "123" };
            var saveModel = new LeadSaveModel
                                {
                                    Name = "name", Address = "addr", Email = "email@email.email", MobileNumber = "12345", PinCode = "123", SubAreaId = 1
                                };
            var viewModel = new LeadViewModel();
            leadsMock.SaveReturn = viewModel;

            var resultLead = await this.leadsService.Save(saveModel).ConfigureAwait(false);
            Assert.True(leadsMock.IsSaveCalled);
        }

        [Fact]
        public async void WhenSaveModelWithoutNameIsSavedValidationExceptionIsThrown()
        {
            var saveModel = new LeadSaveModel
                 {
                     Name = null, Address = "addr", Email = "email@email.email", MobileNumber = "12345", PinCode = "123", SubAreaId = 1
                 };
            await Assert.ThrowsAsync<ArgumentException>(
                async () => await this.leadsService.Save(saveModel).ConfigureAwait(false));

            saveModel.Name = "";
            await Assert.ThrowsAsync<ArgumentException>(
                async () => await this.leadsService.Save(saveModel).ConfigureAwait(false));

            saveModel.Name = " ";
            await Assert.ThrowsAsync<ArgumentException>(
                async () => await this.leadsService.Save(saveModel).ConfigureAwait(false));
        }

        
        [Fact]
        public async void WhenSaveModelWithoutAddressIsSavedValidationExceptionIsThrown()
        {
            var saveModel = new LeadSaveModel
                                {
                                    Name = "name", Address = null, Email = "email@email.email", MobileNumber = "12345", PinCode = "123", SubAreaId = 1
                                };
            await Assert.ThrowsAsync<ArgumentException>(
                async () => await this.leadsService.Save(saveModel).ConfigureAwait(false));

            saveModel.Address = "";
            await Assert.ThrowsAsync<ArgumentException>(
                async () => await this.leadsService.Save(saveModel).ConfigureAwait(false));

            saveModel.Address = " ";
            await Assert.ThrowsAsync<ArgumentException>(
                async () => await this.leadsService.Save(saveModel).ConfigureAwait(false));
        }

        [Fact]
        public async void WhenSaveModelWithoutPinCodeIsSavedValidationExceptionIsThrown()
        {
            var saveModel = new LeadSaveModel
                                {
                                    Name = "name", Address = "addr", Email = "email@email.email", MobileNumber = "12345", PinCode = null, SubAreaId = 1
                                };
            await Assert.ThrowsAsync<ArgumentException>(
                async () => await this.leadsService.Save(saveModel).ConfigureAwait(false));

            saveModel.PinCode = "";
            await Assert.ThrowsAsync<ArgumentException>(
                async () => await this.leadsService.Save(saveModel).ConfigureAwait(false));

            saveModel.PinCode = " ";
            await Assert.ThrowsAsync<ArgumentException>(
                async () => await this.leadsService.Save(saveModel).ConfigureAwait(false));
        }

        [Fact]
        public async void WhenSaveModelWithWhitespacesIsSavedValidationFixesIt()
        {
            subAreasMock.GetByIdReturn = new SubAreaViewModel{ PinCode = "1234" };
            var saveModel = new LeadSaveModel
                                {
                                    Name = " name  ", Address = "  addr ", Email = "email@email.email", MobileNumber = "12345", PinCode = "   1234 \t", SubAreaId = 1
                                };
             await this.leadsService.Save(saveModel).ConfigureAwait(false);

            Assert.Equal("name", saveModel.Name);
            Assert.Equal("addr", saveModel.Address);
            Assert.Equal("1234", saveModel.PinCode);
        }

        [Fact]
        public async void WhenInvalidPinCodeSubAreaIdCombinationIsSavedThrowException()
        {
            var saveModel = new LeadSaveModel
                                {
                                    Name = "name", Address = "addr", Email = "email@email.email", MobileNumber = "12345", PinCode = "1234", SubAreaId = 1
                                };

            subAreasMock.GetByIdReturn = null;

            await Assert.ThrowsAsync<ArgumentException>(
                async () => await this.leadsService.Save(saveModel).ConfigureAwait(false));

            subAreasMock.GetByIdReturn = new SubAreaViewModel{ PinCode = "5678" };

            await Assert.ThrowsAsync<ArgumentException>(
                async () => await this.leadsService.Save(saveModel).ConfigureAwait(false));
        }
    }
}
