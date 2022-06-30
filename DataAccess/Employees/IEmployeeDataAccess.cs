using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DataAccess.Employees
{
    /// <summary>
    /// Represents a DAO for employees.
    /// </summary>
    public interface IEmployeeDataAccess
    {
        /// <summary>
        /// Inserts a new employee to a data storage asynchronous.
        /// </summary>
        /// <param name="employee">A <see cref="EmployeeTransferObject"/>.</param>
        /// <returns>A data storage identifier of a new employee.</returns>
        Task<int> InsertEmployeeAsync(EmployeeTransferObject employee);

        /// <summary>
        /// Deletes a employee from a data storage asynchronous.
        /// </summary>
        /// <param name="employeeId">A employee identifier.</param>
        /// <returns>True if a employee is deleted; otherwise false.</returns>
        Task<bool> DeleteEmployeeAsync(int employeeId);

        /// <summary>
        /// Updates a employee in a data storage asynchronous.
        /// </summary>
        /// <param name="employee">A <see cref="EmployeeTransferObject"/>.</param>
        /// <returns>True if a employee is updated; otherwise false.</returns>
        Task<bool> UpdateEmployeeAsync(EmployeeTransferObject employee);

        /// <summary>
        /// Selects a employees using specified offset and limit in a data storage asynchronous.
        /// </summary>
        /// <param name="offset">An offset of the first object.</param>
        /// <param name="limit">A limit of returned objects.</param>
        /// <returns>A <see cref="List{T}"/> of <see cref="EmployeeTransferObject"/>.</returns>
        Task<IList<EmployeeTransferObject>> SelectEmployeesAsync(int offset, int limit);

        /// <summary>
        /// Finds a employee with identifier asynchronous.
        /// </summary>
        /// <param name="id">A employee identifier.</param>
        /// <returns>A <see cref="EmployeeTransferObject"/> with specified identifier.</returns>
        Task<EmployeeTransferObject> FindByIdAsync(int id);

        /// <summary>
        /// Totals the records asynchronous.
        /// </summary>
        /// <returns></returns>
        Task<int> TotalRecordsAsync();
    }
}
