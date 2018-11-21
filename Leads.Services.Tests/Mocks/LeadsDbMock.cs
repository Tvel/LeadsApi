﻿using Leads.DbAdapter;
using Leads.Models;
using System.Threading.Tasks;

namespace Leads.Services.Tests.Mocks
{
    class LeadsDbMock : ILeadsDb
    {
        public LeadViewModel GetReturn { get; set; }
        public LeadViewModel SaveReturn { get; set; }

        public bool IsSaveCalled { get; set; } = false;
        public bool IsGetByIdCalled { get; set; } = false;

        public Task<LeadViewModel> GetById(int Id)
        {
            IsGetByIdCalled = true;
            return Task.FromResult<LeadViewModel>(GetReturn);
        }

        public Task<LeadViewModel> Save(LeadSaveModel lead)
        {
            IsSaveCalled = true;
            return Task.FromResult<LeadViewModel>(SaveReturn);
        }
    }
}
