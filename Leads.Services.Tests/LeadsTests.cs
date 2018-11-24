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
        public async void WhenYouGetByIdDb_ReturnsCorrectViewModel()
        {
            var lead = new LeadViewModel();
            this.leadsMock.GetReturn = lead;

            var resultLead = await this.leadsService.Get(Guid.NewGuid()).ConfigureAwait(false);
            Assert.Equal(resultLead, lead);
            Assert.True(this.leadsMock.IsGetByIdCalled);
        }

        [Fact]
        public async void WhenYouGetByIdDb_ReturnsSubAreaAsWell()
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
        public async void WhenYouGetByInvalidIdDb_ReturnsNull()
        {
            leadsMock.GetReturn = null;

            var resultLead = await this.leadsService.Get(Guid.NewGuid()).ConfigureAwait(false);
            Assert.Null(resultLead);
        }

        [Fact]
        public async void WhenNullIsSentToSaveModel_ExceptionIsThrown()
        {
            await Assert.ThrowsAsync<ArgumentNullException>(
                async () => await this.leadsService.Save(null).ConfigureAwait(false));
        }

        [Fact]
        public async void WhenValidSaveModelIsSaved_ResultIsId()
        {
            subAreasMock.GetByIdReturn = new SubAreaViewModel{ PinCode = "123" };
            var saveModel = new LeadSaveModel
                                {
                Name = "name", Address = "addr", Email = "email@email.email", MobileNumber = "12345", PinCode = "123", SubAreaId = 1
                                };
            leadsMock.SaveReturn = Guid.NewGuid();

            var resultLead = await this.leadsService.Save(saveModel).ConfigureAwait(false);
            Assert.Equal(resultLead, leadsMock.SaveReturn);
        }

        [Fact]
        public async void WhenValidSaveModelIsSaved_DbIsCalled()
        {
            subAreasMock.GetByIdReturn = new SubAreaViewModel{ PinCode = "123" };
            var saveModel = new LeadSaveModel
                                {
                                    Name = "name", Address = "addr", Email = "email@email.email", MobileNumber = "12345", PinCode = "123", SubAreaId = 1
                                };
            leadsMock.SaveReturn = Guid.NewGuid();

            var resultLead = await this.leadsService.Save(saveModel).ConfigureAwait(false);
            Assert.True(leadsMock.IsSaveCalled);
        }

        [Fact]
        public async void WhenSaveModelWithoutNameIsSaved_ValidationExceptionIsThrown()
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
        public async void WhenSaveModelWithoutAddressIsSaved_ValidationExceptionIsThrown()
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
        public async void WhenSaveModelWithoutPinCodeIsSaved_ValidationExceptionIsThrown()
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
        public async void WhenSaveModelWithWhitespacesIsSaved_ValidationFixesIt()
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
        public async void WhenInvalidPinCodeSubAreaIdCombinationIsSaved_ThrowException()
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
