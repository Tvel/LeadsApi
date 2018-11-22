namespace Leads.Database.File
{
    using System;
    using System.IO;
    using System.Threading.Tasks;

    using Leads.DbAdapter;
    using Leads.Models;

    using Newtonsoft.Json;

    public class LeadsFileDb : ILeadsDb
    {
        private string path;

        public LeadsFileDb(string directory = "FileDb\\Leads")
        {
            this.path = directory;
        }

        public async Task<LeadViewModel> Save(LeadSaveModel lead)
        {
            Guid id = Guid.NewGuid();
            var jsonObj = new {
                                  Id = id,
                                  Address = lead.Address,
                                  Email = lead.Email,
                                  MobileNumber = lead.MobileNumber,
                                  Name = lead.Name,
                                  PinCode = lead.PinCode,
                                  SubAreaId = lead.SubAreaId
                              };
            var json = JsonConvert.SerializeObject(jsonObj);

            var filePath = path + "\\" + id + ".json";
            var file = new FileInfo(filePath);
            file.Directory?.Create(); // If the directory already exists, this method does nothing.
            await File.WriteAllTextAsync(filePath, json);

            return new LeadViewModel
                       {
                           Id = id,
                           Address = lead.Address,
                           Email = lead.Email,
                           MobileNumber = lead.MobileNumber,
                           Name = lead.Name,
                           PinCode = lead.PinCode,
                           SubAreaId = lead.SubAreaId
                       };
        }

        public async Task<LeadViewModel> GetById(Guid id)
        {
            var stringContent = await File.ReadAllTextAsync(path + "\\" + id + ".json")
                                    .ConfigureAwait(false);

            return JsonConvert.DeserializeObject<LeadViewModel>(stringContent);
        }
    }
}
