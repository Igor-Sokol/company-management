using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DataAccess.Companies
{
    /// <summary>
    /// Represents a TO for company.
    /// </summary>
    public class CompanyTransferObject
    {
        /// <summary>
        /// Gets or sets the identifier.
        /// </summary>
        /// <value>
        /// The identifier.
        /// </value>
        public int Id { get; set; }

        /// <summary>
        /// Gets or sets the name.
        /// </summary>
        /// <value>
        /// The name.
        /// </value>
        public string Name { get; set; }

        /// <summary>
        /// Gets or sets the lfo.
        /// </summary>
        /// <value>
        /// The lfo.
        /// </value>
        public string LFO { get; set; }
    }
}
