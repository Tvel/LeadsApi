﻿using Leads.DbAdapter;
using Leads.Models;
using System.Threading.Tasks;

namespace Leads.Services.Tests.Mocks
{
    using System;

    class LeadsDbMock : ILeadsDb
    {
        public LeadViewModel GetReturn { get; set; }
        public Guid SaveReturn { get; set; }

        public bool IsSaveCalled { get; set; } = false;
        public bool IsGetByIdCalled { get; set; } = false;

        public Task<LeadViewModel> GetById(Guid id)
        {
            IsGetByIdCalled = true;
            return Task.FromResult<LeadViewModel>(GetReturn);
        }

        public Task<Guid> Save(LeadSaveModel lead)
        {
            IsSaveCalled = true;
            return Task.FromResult<Guid>(SaveReturn);
        }
    }
}
