using System;
using Leads.DbAdapter;
using Leads.Models;

namespace Leads.Services.Tests.Mocks
{
    using System.Threading.Tasks;

    class LeadsDbMock : ILeadsDb
    {
        public LeadViewModel GetReturn { get; set; }
        public LeadViewModel SaveReturn { get; set; }

        public Task<LeadViewModel> GetById(int Id)
        {
            return Task.FromResult<LeadViewModel>(GetReturn);
        }

        public Task<LeadViewModel> Save(LeadSaveModel lead)
        {
            return Task.FromResult<LeadViewModel>(SaveReturn);
        }
    }
}
