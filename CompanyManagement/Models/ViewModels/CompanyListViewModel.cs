using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace CompanyManagement.Models.ViewModels
{
    public class CompanyListViewModel
    {
        public IEnumerable<CompanyViewModel> Companies { get; set; }

        public PageInfo PageInfo { get; set; }
    }
}
