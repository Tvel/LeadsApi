namespace Leads.Models
{
    using System.Dynamic;

    public class LeadViewModel : LeadSaveModel
    {
        public int Id { get; set; }
        public SubAreaViewModel SubArea { get; set; }
    }
}
