namespace Leads.DbAdapter
{
    using System.Collections.Generic;
    using System.Threading.Tasks;

    using Models;

    public interface ISubAreasDb
    {
        Task<List<SubAreaViewModel>> GetAll();

        Task<List<SubAreaViewModel>> GetByPinCode(int pinCode);

        Task<SubAreaViewModel> GetById(int Id);
    }
}
