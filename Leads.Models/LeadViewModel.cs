namespace Leads.Models
{
    using System;

    public class LeadViewModel : LeadSaveModel
    {
        public Guid Id { get; set; }
        public SubAreaViewModel SubArea { get; set; }
    }
}
