namespace Leads.WebApi.Models
{
    using System;

    public class LeadsSaveReturnModel
    {
        public LeadsSaveReturnModel(Guid id)
        {
            Id = id;
        }

        public Guid Id { get; private set; }
    }
}
