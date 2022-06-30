using DataAccess.Companies;
using System;
using System.Collections.Generic;
using System.Data.SqlClient;
using System.Globalization;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DataAccess.SqlServer.Companies
{
    /// <inheritdoc/>
    public class CompanySqlServerAccess : ICompanyDataAccess
    {
        private readonly SqlConnection sqlConnection;

        public CompanySqlServerAccess(SqlConnection sqlConnection)
        {
            this.sqlConnection = sqlConnection ?? throw new ArgumentNullException(nameof(sqlConnection), "SqlConnection can not be null.");
        }

        /// <inheritdoc/>
        public async Task<bool> DeleteCompanyAsync(int companyId)
        {
            if (companyId <= 0)
            {
                throw new ArgumentException("CompanyId must be greater than zero.", nameof(companyId));
            }

            const string commandText =
                @"DELETE FROM dbo.Companies WHERE Id = @companyId
                SELECT @@ROWCOUNT";

            using var command = new SqlCommand(commandText, sqlConnection);

            const string companyIdParameter = "@companyId";
            command.Parameters.Add(companyIdParameter, System.Data.SqlDbType.Int);
            command.Parameters[companyIdParameter].Value = companyId;

            await this.sqlConnection.OpenAsync();
            var result = await command.ExecuteScalarAsync();
            await this.sqlConnection.CloseAsync();

            return ((int)result) > 0;
        }

        /// <inheritdoc/>
        public async Task<int> InsertCompanyAsync(CompanyTransferObject company)
        {
            if (company is null)
            {
                throw new ArgumentNullException(nameof(company), "Company can not be null.");
            }

            const string commandText =
                @"INSERT INTO dbo.Companies (Name, LFO) OUTPUT Inserted.Id
                VALUES (@name, @LFO)";

            using var command = new SqlCommand(commandText, sqlConnection);

            AddSqlParameters(company, command);

            await this.sqlConnection.OpenAsync();
            var id = await command.ExecuteScalarAsync();
            await this.sqlConnection.CloseAsync();

            return (int)id;
        }

        /// <inheritdoc/>
        public async Task<IList<CompanyTransferObject>> SelectCompaniesAsync(int offset, int limit)
        {
            if (offset < 0)
            {
                throw new ArgumentException("Must be greater than zero or equals zero.", nameof(offset));
            }

            if (limit < 1)
            {
                throw new ArgumentException("Must be greater than zero.", nameof(limit));
            }

            const string commandTemplate =
                @"SELECT c.Id, c.Name, c.LFO
                FROM dbo.Companies as c
                ORDER BY c.Id
                OFFSET {0} ROWS
                FETCH FIRST {1} ROWS ONLY";

            string commandText = string.Format(CultureInfo.CurrentCulture, commandTemplate, offset, limit);
            return await this.ExecuteReaderAsync(commandText);
        }

        /// <inheritdoc/>
        public async Task<bool> UpdateCompanyAsync(CompanyTransferObject company)
        {
            if (company is null)
            {
                throw new ArgumentNullException(nameof(company), "Company can not be null.");
            }

            const string commandText =
                @"UPDATE dbo.Companies
                SET Name = @name, LFO = @LFO
                WHERE Id = @companyId
                SELECT @@ROWCOUNT";

            using var command = new SqlCommand(commandText, sqlConnection);

            AddSqlParameters(company, command);

            await this.sqlConnection.OpenAsync();
            int result = (int)(await command.ExecuteScalarAsync());
            await this.sqlConnection.CloseAsync();

            return result > 0;
        }

        /// <inheritdoc/>
        public async Task<CompanyTransferObject> FindByIdAsync(int id)
        {
            if (id <= 0)
            {
                throw new ArgumentNullException(nameof(id), "Id must be grater than zero.");
            }

            const string commandText =
                @"SELECT c.Id, c.Name, c.LFO
                FROM dbo.Companies as c
                WHERE c.Id = @companyId";

            using var command = new SqlCommand(commandText, sqlConnection);

            const string companyIdParameter = "@companyId";
            command.Parameters.Add(companyIdParameter, System.Data.SqlDbType.Int);
            command.Parameters[companyIdParameter].Value = id;

            CompanyTransferObject company = null;

            await this.sqlConnection.OpenAsync();
            var reader = await command.ExecuteReaderAsync();

            if (reader.Read())
            {
                company = this.CreateCompany(reader);
            }
            await this.sqlConnection.CloseAsync();

            return company;
        }

        /// <inheritdoc/>
        public async Task<int> TotalRecordsAsync()
        {
            const string commandText = "SELECT COUNT(*) FROM dbo.Companies";

            using var command = new SqlCommand(commandText, this.sqlConnection);

            this.sqlConnection.Open();
            int result = (int)(await command.ExecuteScalarAsync());
            this.sqlConnection.Close();

            return result;
        }

        private static void AddSqlParameters(CompanyTransferObject company, SqlCommand command)
        {
            const string companyIdParameter = "@companyId";
            command.Parameters.Add(companyIdParameter, System.Data.SqlDbType.Int);
            command.Parameters[companyIdParameter].Value = company.Id;

            const string nameParameter = "@name";
            command.Parameters.Add(nameParameter, System.Data.SqlDbType.NVarChar, 50);
            command.Parameters[nameParameter].Value = company.Name;

            const string lfoParameter = "@LFO";
            command.Parameters.Add(lfoParameter, System.Data.SqlDbType.NVarChar, 50);
            command.Parameters[lfoParameter].Value = company.LFO;
        }

        private async Task<IList<CompanyTransferObject>> ExecuteReaderAsync(string commandText)
        {
            var companies = new List<CompanyTransferObject>();

            using var command = new SqlCommand(commandText, this.sqlConnection);

            await this.sqlConnection.OpenAsync();
            using (var reader = await command.ExecuteReaderAsync())
            {
                while (await reader.ReadAsync())
                {
                    companies.Add(CreateCompany(reader));
                }
            }
            await this.sqlConnection.CloseAsync();

            return companies;
        }

        private CompanyTransferObject CreateCompany(SqlDataReader reader)
        {
            int id = (int)reader["Id"];
            string name = (string)reader["Name"];
            string lfo = (string)reader["LFO"];

            return new CompanyTransferObject() 
            { 
                Id = id, 
                Name = name, 
                LFO = lfo 
            };
        }
    }
}
