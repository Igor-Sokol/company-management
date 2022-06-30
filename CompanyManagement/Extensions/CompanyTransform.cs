using CompanyManagement.Models.ViewModels;
using DataAccess.Companies;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace CompanyManagement.Extensions
{
    public static class CompanyTransform
    {
        public static CompanyViewModel Transform(this CompanyTransferObject company)
        {
            return new CompanyViewModel()
            {
                Id = company.Id,
                Name = company.Name,
                LFO = company.LFO,
            };
        }

        public static CompanyTransferObject Transform(this CompanyViewModel company)
        {
            return new CompanyTransferObject()
            {
                Id = company.Id,
                Name = company.Name,
                LFO = company.LFO,
            };
        }
    }
}
