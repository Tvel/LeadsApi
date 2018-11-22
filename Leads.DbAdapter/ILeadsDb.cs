﻿namespace Leads.DbAdapter
{
    using System;
    using System.Threading.Tasks;

    using Models;

    public interface ILeadsDb
    {
        Task<LeadViewModel> Save(LeadSaveModel lead);

        Task<LeadViewModel> GetById(Guid id);
    }
}
