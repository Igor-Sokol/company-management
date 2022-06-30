using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Threading.Tasks;

namespace CompanyManagement.Models
{
    public enum Post : byte
    {
        [Display(Name = "Developer")]
        Developer,
        [Display(Name = "Tester")]
        Tester,
        [Display(Name = "Business Analyst")]
        BusinessAnalyst,
        [Display(Name = "Manager")]
        Manager,
    }
}
