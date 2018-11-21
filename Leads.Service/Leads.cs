[assembly: System.Runtime.CompilerServices.InternalsVisibleTo("Leads.Services.Tests")]
namespace Leads.Services
{
    using System;
    using System.Threading.Tasks;

    using DbAdapter;
    using Models;

    public class Leads
    {
        private readonly ILeadsDb leadsDb;
        private readonly ISubAreasDb subAreasDb;

        public Leads(ILeadsDb leadsDb, ISubAreasDb subAreasDb)
        {
            this.leadsDb = leadsDb;
            this.subAreasDb = subAreasDb;
        }

        public async Task<bool> Save(LeadSaveModel lead)
        {
            ValidateSaveModel(lead);
            await ValidateSubArea(lead.SubAreaId, lead.PinCode)
                .ConfigureAwait(false);
            await this.leadsDb.Save(lead)
                .ConfigureAwait(false);

            return true;
        }

        private async Task ValidateSubArea(int leadSubAreaId, string leadPinCode)
        {
            var subarea = await this.subAreasDb.GetById(leadSubAreaId)
                              .ConfigureAwait(false);
            if (subarea == null || subarea.PinCode != leadPinCode)
            {
                throw new ArgumentException("SubArea is invalid");
            }
        }

        public Task<LeadViewModel> Get(int id)
        {
            return this.leadsDb.GetById(id);
        }

        private void ValidateSaveModel(LeadSaveModel candidate)
        {
            if (candidate is null)
            {
                throw new ArgumentNullException(nameof(candidate), "cannot be null");
            }

            if (string.IsNullOrWhiteSpace(candidate.Name))
            {
                throw new ArgumentException("Name cannot be empty", nameof(candidate.Name));
            }

            if (string.IsNullOrWhiteSpace(candidate.PinCode))
            {
                throw new ArgumentException("PinCode cannot be empty", nameof(candidate.PinCode));
            }

            if (string.IsNullOrWhiteSpace(candidate.Address))
            {
                throw new ArgumentException("Address cannot be empty", nameof(candidate.Address));
            }

            candidate.Name = candidate.Name.Trim();
            candidate.PinCode = candidate.PinCode.Trim();
            candidate.Address = candidate.Address.Trim();
        }
    }
}
