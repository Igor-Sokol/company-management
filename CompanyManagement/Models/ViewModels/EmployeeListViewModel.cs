using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace CompanyManagement.Models.ViewModels
{
    public class EmployeeListViewModel
    {
        public IEnumerable<EmployeeViewModel> Employees { get; set; }

        public PageInfo PageInfo { get; set; }
    }
}
