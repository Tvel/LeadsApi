using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;

namespace Leads.WebApi.Controllers
{
    using Leads.DbAdapter;
    using Leads.Models;
    using Leads.Services;

    [Route("api/[controller]")]
    [ApiController]
    public class SubAreasController : ControllerBase
    {
        private readonly SubAreasService subAreasService;

        public SubAreasController(SubAreasService subAreasService)
        {
            this.subAreasService = subAreasService;
        }

        // GET: api/SubAreas
        [HttpGet]
        public Task<List<SubAreaViewModel>> Get()
        {
            return this.subAreasService.GetAll();
        }

        // GET: api/SubAreas/Filter/PinCode/5
        [HttpGet("Filter/PinCode/{pinCode}", Name = "GetByPinCode")]
        public Task<List<SubAreaViewModel>> Get(string pinCode)
        {
            return this.subAreasService.GetByPinCode(pinCode);
        }
    }
}
