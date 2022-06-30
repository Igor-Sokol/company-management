using CompanyManagement.Extensions;
using CompanyManagement.Models;
using CompanyManagement.Models.ViewModels;
using DataAccess.Companies;
using DataAccess.Employees;
using Microsoft.AspNetCore.Mvc;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace CompanyManagement.Controllers
{
    public class EmployeeController : Controller
    {
        private readonly IEmployeeDataAccess employees;
        private readonly ICompanyDataAccess companies;

        private readonly int pageSize = 10;

        public EmployeeController(IEmployeeDataAccess employeeAccess, ICompanyDataAccess companiyAccess)
        {
            this.employees = employeeAccess ?? throw new ArgumentNullException(nameof(employeeAccess), "employeeAccess can not be null.");
            this.companies = companiyAccess ?? throw new ArgumentNullException(nameof(companiyAccess), "companiyAccess can not be null.");
        }

        public async Task<IActionResult> Index(int page = 1)
        {
            return View(new EmployeeListViewModel()
            {
                Employees = (await employees.SelectEmployeesAsync((page - 1) * this.pageSize, pageSize)).Select(e => e.Transform(this.companies).Result),
                PageInfo = new PageInfo() { CurrentPage = page, ItemsPerPage = this.pageSize, TotalItems = await this.employees.TotalRecordsAsync() },
            });
        }

        [HttpPost]
        public async Task<IActionResult> EditEmployee([FromForm] EmployeeViewModel employee, string returnUrl)
        {
            ViewData["Companies"] = (await this.companies.SelectCompaniesAsync(0, await this.companies.TotalRecordsAsync())).Select(c => c.Transform());

            return View((employee, returnUrl));
        }

        [HttpPost]
        public async Task<IActionResult> SaveChanges([FromForm] EmployeeViewModel employee, string returnUrl)
        {
            ModelState["Company.Name"].ValidationState = Microsoft.AspNetCore.Mvc.ModelBinding.ModelValidationState.Skipped;
            ModelState["Company.LFO"].ValidationState = Microsoft.AspNetCore.Mvc.ModelBinding.ModelValidationState.Skipped;

            if (ModelState.IsValid)
            {
                await this.employees.UpdateEmployeeAsync(employee.Transform());
                return Redirect(returnUrl);
            }

            ViewData["Companies"] = (await this.companies.SelectCompaniesAsync(0, await this.companies.TotalRecordsAsync())).Select(c => c.Transform());
            return View("EditEmployee", (employee, returnUrl));
        }

        [HttpPost]
        public async Task<IActionResult> DeleteEmployee([FromForm] EmployeeViewModel employee, string returnUrl)
        {
            await this.employees.DeleteEmployeeAsync(employee.Id);

            return Redirect(returnUrl);
        }

        [HttpGet]
        public async Task<IActionResult> AddEmployee(string returnUrl)
        {
            ViewData["Companies"] = (await this.companies.SelectCompaniesAsync(0, await this.companies.TotalRecordsAsync())).Select(c => c.Transform());

            return View(model: returnUrl);
        }

        [HttpPost]
        public async Task<IActionResult> AddEmployee([FromForm] EmployeeViewModel employee, string returnUrl)
        {
            ModelState["Id"].ValidationState = Microsoft.AspNetCore.Mvc.ModelBinding.ModelValidationState.Skipped;
            ModelState["Company.Name"].ValidationState = Microsoft.AspNetCore.Mvc.ModelBinding.ModelValidationState.Skipped;
            ModelState["Company.LFO"].ValidationState = Microsoft.AspNetCore.Mvc.ModelBinding.ModelValidationState.Skipped;

            if (ModelState.IsValid)
            {
                await this.employees.InsertEmployeeAsync(employee.Transform());
                return Redirect(returnUrl);
            }

            ViewData["Companies"] = (await this.companies.SelectCompaniesAsync(0, await this.companies.TotalRecordsAsync())).Select(c => c.Transform());
            return View(model: returnUrl);
        }
    }
}
