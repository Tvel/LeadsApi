using Leads.DbAdapter;
using System.Collections.Generic;
using Leads.Models;
using System.Threading.Tasks;

namespace Leads.Services.Tests.Mocks
{
    class SubAreasMock : ISubAreasDb
    {
        public List<SubAreaViewModel> GetAllReturn { get; set; }
        public List<SubAreaViewModel> GetByPinCodeReturn { get; set; }
        public SubAreaViewModel GetByIdReturn { get; set; }

        public Task<List<SubAreaViewModel>> GetAll()
        {
            return Task.FromResult(GetAllReturn);
        }

        public Task<List<SubAreaViewModel>> GetByPinCode(int pinCode)
        {
            return Task.FromResult(GetByPinCodeReturn);
        }

        public Task<SubAreaViewModel> GetById(int Id)
        {
            return Task.FromResult(GetByIdReturn);
        }
    }
}
