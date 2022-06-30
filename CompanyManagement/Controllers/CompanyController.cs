using CompanyManagement.Extensions;
using CompanyManagement.Models;
using CompanyManagement.Models.ViewModels;
using DataAccess.Companies;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace CompanyManagement.Controllers
{
    public class CompanyController : Controller
    {
        private readonly ICompanyDataAccess companies;

        private readonly int pageSize = 10;

        public CompanyController(ICompanyDataAccess companies)
        {
            this.companies = companies ?? throw new ArgumentNullException(nameof(companies), "Companies can not be null.");
        }

        public async Task<IActionResult> Index(int page = 1)
        {
            return View(new CompanyListViewModel()
            {
                Companies = (await companies.SelectCompaniesAsync((page - 1) * this.pageSize, this.pageSize)).Select(c => c.Transform()),
                PageInfo = new PageInfo() { CurrentPage = page, ItemsPerPage = pageSize, TotalItems = await this.companies.TotalRecordsAsync() },
            });
        }

        [HttpPost]
        public IActionResult EditCompany([FromForm] CompanyViewModel company, string returnUrl)
        {
            ViewData["returnUrl"] = returnUrl;

            return View((company, returnUrl));
        }

        [HttpPost]
        public async Task<IActionResult> SaveChanges([FromForm] CompanyViewModel company, string returnUrl)
        {
            if (ModelState.IsValid)
            {
                await this.companies.UpdateCompanyAsync(company.Transform());

                return Redirect(returnUrl);
            }

            ViewData["returnUrl"] = returnUrl;
            return View("EditCompany", (company, returnUrl));
        }

        [HttpPost]
        public async Task<IActionResult> DeleteCompany([FromForm] CompanyViewModel company, string returnUrl)
        {
            await this.companies.DeleteCompanyAsync(company.Id);

            return Redirect(returnUrl);
        }

        [HttpGet]
        public IActionResult AddCompany(string returnUrl)
        {
            return View(model: returnUrl);
        }

        [HttpPost]
        public async Task<IActionResult> AddCompany([FromForm] CompanyViewModel company, string returnUrl)
        {
            ModelState["Id"].ValidationState = Microsoft.AspNetCore.Mvc.ModelBinding.ModelValidationState.Skipped;
            if (ModelState.IsValid)
            {
                await this.companies.InsertCompanyAsync(company.Transform());
                return Redirect(returnUrl);
            }

            return View(model: returnUrl);
        }
    }
}
