[assembly: System.Runtime.CompilerServices.InternalsVisibleTo("Leads.Services.Tests")]
namespace Leads.Services
{
    using System;
    using System.Collections.Generic;
    using System.Threading.Tasks;

    using DbAdapter;
    using Models;

    public class SubAreas
    {
        private readonly ISubAreasDb subAreasDb;

        public SubAreas(ISubAreasDb subAreasDb)
        {
            this.subAreasDb = subAreasDb;
        }

        public Task<List<SubAreaViewModel>> GetAll()
        {
            return this.subAreasDb.GetAll();
        }

        public Task<List<SubAreaViewModel>> GetByPinCode(string pinCode)
        {
            if (string.IsNullOrWhiteSpace(pinCode))
            {
                throw new ArgumentException("PinCode cannot be null or empty");
            }

            return this.subAreasDb.GetByPinCode(pinCode);
        }
    }
}
