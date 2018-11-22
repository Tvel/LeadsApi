using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;

namespace Leads.WebApi.Controllers
{
    using Leads.Models;
    using Leads.Services;
    using Leads.WebApi.Models;

    [Route("api/[controller]")]
    [ApiController]
    public class LeadsController : ControllerBase
    {
        private readonly LeadsService leadsService;

        public LeadsController(LeadsService leadsService)
        {
            this.leadsService = leadsService;
        }

        // GET: api/Leads/e7c60642-6be2-41ec-b329-b0ef3bb8f556
        [HttpGet("{id}", Name = "Get")]
        public async Task<ActionResult<LeadViewModel>> Get(Guid id)
        {
            var lead = await leadsService.Get(id);
            if (lead is null)
            {
                return this.NotFound();
            }

            return this.Ok(lead);
        }

        // POST: api/Leads
        [HttpPost]
        public async Task<ActionResult> Post([FromBody] LeadsSaveViewModel candidate)
        {
            try
            {
                var leadSaveModel = new LeadSaveModel
                                        {
                                            Name = candidate.Name,
                                            Address = candidate.Address,
                                            PinCode = candidate.PinCode,
                                            MobileNumber = candidate.MobileNumber,
                                            Email = candidate.Email,
                                            SubAreaId = candidate.SubAreaId.Value
                                        };

                var result = await leadsService.Save(leadSaveModel);
                if (result)
                {
                    return this.Ok("Ok");
                }
            }
            catch (Exception e)
            {
                return this.BadRequest(e.Message);
            }

            return this.BadRequest("Cannot Save");
        }
    }
}
