using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DataAccess.Employees
{
    /// <summary>
    /// Represents a TO for employees.
    /// </summary>
    public class EmployeeTransferObject
    {
        /// <summary>
        /// Gets or sets the identifier.
        /// </summary>
        /// <value>
        /// The identifier.
        /// </value>
        public int Id { get; set; }

        /// <summary>
        /// Gets or sets the surname.
        /// </summary>
        /// <value>
        /// The surname.
        /// </value>
        public string Surname { get; set; }

        /// <summary>
        /// Gets or sets the name.
        /// </summary>
        /// <value>
        /// The name.
        /// </value>
        public string Name { get; set; }

        /// <summary>
        /// Gets or sets the patronymic.
        /// </summary>
        /// <value>
        /// The patronymic.
        /// </value>
        public string Patronymic { get; set; }

        /// <summary>
        /// Gets or sets the hire date.
        /// </summary>
        /// <value>
        /// The hire date.
        /// </value>
        public DateTime HireDate { get; set; }

        /// <summary>
        /// Gets or sets the post.
        /// </summary>
        /// <value>
        /// The post.
        /// </value>
        public string Post { get; set; }

        /// <summary>
        /// Gets or sets the company identifier.
        /// </summary>
        /// <value>
        /// The company identifier.
        /// </value>
        public int CompanyId { get; set; }
    }
}
