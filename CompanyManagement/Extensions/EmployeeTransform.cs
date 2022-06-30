using CompanyManagement.Models;
using CompanyManagement.Models.ViewModels;
using DataAccess.Companies;
using DataAccess.Employees;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace CompanyManagement.Extensions
{
    public static class EmployeeTransform
    {
        public async static Task<EmployeeViewModel> Transform(this EmployeeTransferObject employee, ICompanyDataAccess companyAccess)
        {
            return new EmployeeViewModel()
            {
                Id = employee.Id,
                Surname = employee.Surname,
                Name = employee.Name,
                Patronymic = employee.Patronymic,
                HireDate = employee.HireDate,
                Post = (Post)Enum.Parse(typeof(Post), employee.Post, true),
                Company = (await companyAccess.FindByIdAsync(employee.CompanyId)).Transform(),
            };
        }

        public static EmployeeTransferObject Transform(this EmployeeViewModel employee)
        {
            return new EmployeeTransferObject()
            {
                Id = employee.Id,
                Surname = employee.Surname,
                Name = employee.Name,
                Patronymic = employee.Patronymic,
                HireDate = employee.HireDate,
                Post = employee.Post.ToString(),
                CompanyId = employee.Company.Id,
            };
        }
    }
}
