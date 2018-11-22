﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace Leads.WebApi.Models
{
    public class LeadsSaveViewModel
    {
        public string Name { get; set; }
        public string PinCode { get; set; }
        public int? SubAreaId { get; set; }
        public string Address { get; set; }
        public string MobileNumber { get; set; }
        public string Email { get; set; }
    }
}
