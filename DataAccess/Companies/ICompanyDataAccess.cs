using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DataAccess.Companies
{
    /// <summary>
    /// Represents a DAO for companies.
    /// </summary>
    public interface ICompanyDataAccess
    {
        /// <summary>
        /// Inserts a new company to a data storage asynchronous.
        /// </summary>
        /// <param name="company">A <see cref="CompanyTransferObject"/>.</param>
        /// <returns>A data storage identifier of a new employee.</returns>
        Task<int> InsertCompanyAsync(CompanyTransferObject company);

        /// <summary>
        /// Deletes a company from a data storage asynchronous.
        /// </summary>
        /// <param name="companyId">A company identifier.</param>
        /// <returns>True if a employee is deleted; otherwise false.</returns>
        Task<bool> DeleteCompanyAsync(int companyId);

        /// <summary>
        /// Updates a company in a data storage asynchronous.
        /// </summary>
        /// <param name="company">A <see cref="CompanyTransferObject"/>.</param>
        /// <returns>True if a employee is updated; otherwise false.</returns>
        Task<bool> UpdateCompanyAsync(CompanyTransferObject company);

        /// <summary>
        /// Selects a companies using specified offset and limit in a data storage asynchronous.
        /// </summary>
        /// <param name="offset">An offset of the first object.</param>
        /// <param name="limit">A limit of returned objects.</param>
        /// <returns>A <see cref="List{T}"/> of <see cref="CompanyTransferObject"/>.</returns>
        Task<IList<CompanyTransferObject>> SelectCompaniesAsync(int offset, int limit);

        /// <summary>
        /// Finds a company with identifier asynchronous.
        /// </summary>
        /// <param name="id">A company identifier.</param>
        /// <returns>A <see cref="CompanyTransferObject"/> with specified identifier.</returns>
        Task<CompanyTransferObject> FindByIdAsync(int id);

        /// <summary>
        /// Totals the records asynchronous.
        /// </summary>
        /// <returns></returns>
        Task<int> TotalRecordsAsync();
    }
}
