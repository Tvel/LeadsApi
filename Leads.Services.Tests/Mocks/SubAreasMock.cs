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

        public bool IsGetAllCalled { get; set; } = false;
        public bool IsGetByPinCodeCalled { get; set; } = false;

        public Task<List<SubAreaViewModel>> GetAll()
        {
            IsGetAllCalled = true;
            return Task.FromResult(GetAllReturn);
        }

        public Task<List<SubAreaViewModel>> GetByPinCode(string pinCode)
        {
            IsGetByPinCodeCalled = true;
            return Task.FromResult(GetByPinCodeReturn);
        }

        public Task<SubAreaViewModel> GetById(int Id)
        {
            return Task.FromResult(GetByIdReturn);
        }
    }
}
