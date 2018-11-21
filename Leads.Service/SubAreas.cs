using System.Runtime.CompilerServices;

[assembly: InternalsVisibleTo("Leads.Services.Tests")]
namespace Leads.Services
{
    using System;
    using System.Collections.Generic;
    using System.Threading.Tasks;

    using DbAdapter;
    using Models;

    class SubAreas
    {
        private readonly ISubAreasDb subAreasDbDb;

        public SubAreas(ISubAreasDb subAreasDbDb)
        {

        }

        public async Task<List<SubAreaViewModel>> GetAll()
        {
            throw  new NotImplementedException();
        }

        public async Task<List<SubAreaViewModel>> GetByPinCode(int pinCodeId)
        {
            throw  new NotImplementedException();
        }
    }
}
