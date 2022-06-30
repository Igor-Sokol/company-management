using DataAccess.Employees;
using System;
using System.Collections.Generic;
using System.Data.SqlClient;
using System.Globalization;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DataAccess.SqlServer.Employees
{
    /// <inheritdoc/>
    public class EmployeeSqlServerAccess : IEmployeeDataAccess
    {
        private SqlConnection sqlConnection;

        public EmployeeSqlServerAccess(SqlConnection sqlConnection)
        {
            this.sqlConnection = sqlConnection ?? throw new ArgumentNullException(nameof(sqlConnection), "SqlConnection can not be null.");
        }

        /// <inheritdoc/>
        public async Task<bool> DeleteEmployeeAsync(int employeeId)
        {
            if (employeeId <= 0)
            {
                throw new ArgumentNullException(nameof(employeeId), "EmployeeId must be greater than zero.");
            }

            const string commandText =
                @"DELETE FROM dbo.Employees WHERE Id = @employeeId
                SELECT @@ROWCOUNT";

            using var command = new SqlCommand(commandText, this.sqlConnection);

            const string employeeIdParameter = "@employeeId";
            command.Parameters.Add(employeeIdParameter, System.Data.SqlDbType.Int);
            command.Parameters[employeeIdParameter].Value = employeeId;

            await this.sqlConnection.OpenAsync();
            int result = (int)(await command.ExecuteScalarAsync());
            await this.sqlConnection.CloseAsync();

            return result > 0;
        }

        /// <inheritdoc/>
        public async Task<int> InsertEmployeeAsync(EmployeeTransferObject employee)
        {
            if (employee is null)
            {
                throw new ArgumentNullException(nameof(employee), "Employee can not be null.");
            }

            const string commandText =
                @"INSERT INTO dbo.Employees (Surname, Name, Patronymic, HireDate, Post, CompanyId) OUTPUT Inserted.Id
                VALUES (@surname, @name, @patronymic, @hireDate, @post, @companyId)";

            using var command = new SqlCommand(commandText, this.sqlConnection);

            AddSqlParameters(employee, command);

            await this.sqlConnection.OpenAsync();
            int result = (int)(await command.ExecuteScalarAsync());
            await this.sqlConnection.CloseAsync();

            return result;
        }

        /// <inheritdoc/>
        public async Task<IList<EmployeeTransferObject>> SelectEmployeesAsync(int offset, int limit)
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
                @"SELECT e.Id, e.Surname, e.Name, e.Patronymic, e.HireDate, e.Post, e.CompanyId
                FROM dbo.Employees as e
                ORDER BY e.Id
                OFFSET {0} ROWS
                FETCH FIRST {1} ROWS ONLY";

            string commandText = string.Format(CultureInfo.CurrentCulture, commandTemplate, offset, limit);

            return await ExecuteReaderAsync(commandText);
        }

        /// <inheritdoc/>
        public async Task<bool> UpdateEmployeeAsync(EmployeeTransferObject employee)
        {
            if (employee is null)
            {
                throw new ArgumentNullException(nameof(employee), "Employee can not be null.");
            }

            const string commandText =
                @"UPDATE dbo.Employees
                SET Surname = @surname, Name = @name, Patronymic = @patronymic, HireDate = @hireDate, Post = @post, CompanyId = @companyId
                WHERE Id = @employeeId
                SELECT @@ROWCOUNT";

            using var command = new SqlCommand(commandText, sqlConnection);

            AddSqlParameters(employee, command);

            await this.sqlConnection.OpenAsync();
            int result = (int)(await command.ExecuteScalarAsync());
            await this.sqlConnection.CloseAsync();

            return result > 0;
        }

        /// <inheritdoc/>
        public async Task<EmployeeTransferObject> FindByIdAsync(int id)
        {
            if (id <= 0)
            {
                throw new ArgumentNullException(nameof(id), "Id must be greater than zero.");
            }

            const string commandText =
                @"SELECT e.Id, e.Surname, e.Name, e.Patronymic, e.HireDate, e.Post, e.CompanyId
                FROM dbo.Employees as e
                WHERE e.Id = @employeeId";

            using var command = new SqlCommand(commandText, this.sqlConnection);

            const string employeeIdParameter = "@employeeId";
            command.Parameters.Add(employeeIdParameter, System.Data.SqlDbType.Int);
            command.Parameters[employeeIdParameter].Value = id;

            EmployeeTransferObject employee = null;

            await this.sqlConnection.OpenAsync();
            using (var reader = await command.ExecuteReaderAsync())
            {
                if (await reader.ReadAsync())
                {
                    employee = CreateEmployee(reader);
                }
            }
            await this.sqlConnection.CloseAsync();

            return employee;
        }

        /// <inheritdoc/>
        public async Task<int> TotalRecordsAsync()
        {
            const string commandText = "SELECT COUNT(*) FROM dbo.Employees";

            using var command = new SqlCommand(commandText, this.sqlConnection);

            this.sqlConnection.Open();
            int result = (int)(await command.ExecuteScalarAsync());
            this.sqlConnection.Close();

            return result;
        }

        private static void AddSqlParameters(EmployeeTransferObject employee, SqlCommand command)
        {
            const string employeeId = "@employeeId";
            command.Parameters.Add(employeeId, System.Data.SqlDbType.Int);
            command.Parameters[employeeId].Value = employee.Id;

            const string name = "@name";
            command.Parameters.Add(name, System.Data.SqlDbType.NVarChar);
            command.Parameters[name].Value = employee.Name;

            const string surname = "@surname";
            command.Parameters.Add(surname, System.Data.SqlDbType.NVarChar);
            command.Parameters[surname].Value = employee.Surname;

            const string patronymic = "@patronymic";
            command.Parameters.Add(patronymic, System.Data.SqlDbType.NVarChar);
            command.Parameters[patronymic].Value = employee.Patronymic;

            const string post = "@post";
            command.Parameters.Add(post, System.Data.SqlDbType.NVarChar, 50);
            command.Parameters[post].Value = employee.Post;

            const string hireDate = "@hireDate";
            command.Parameters.Add(hireDate, System.Data.SqlDbType.Date);
            command.Parameters[hireDate].Value = employee.HireDate;

            const string companyId = "@companyId";
            command.Parameters.Add(companyId, System.Data.SqlDbType.Int);
            command.Parameters[companyId].Value = employee.CompanyId;
        }

        private async Task<IList<EmployeeTransferObject>> ExecuteReaderAsync(string commandText)
        {
            List<EmployeeTransferObject> employees = new List<EmployeeTransferObject>();

            using var command = new SqlCommand(commandText, this.sqlConnection);

            await this.sqlConnection.OpenAsync();
            using (var reader = await command.ExecuteReaderAsync())
            {
                while (await reader.ReadAsync())
                {
                    employees.Add(CreateEmployee(reader));
                }
            }
            this.sqlConnection.Close();

            return employees;
        }

        private EmployeeTransferObject CreateEmployee(SqlDataReader reader)
        {
            int id = (int)reader["Id"];
            string name = (string)reader["Name"];
            string surname = (string)reader["Surname"];
            string patronymic = (string)reader["Patronymic"];
            DateTime hiredate = (DateTime)reader["HireDate"];
            string post = (string)reader["Post"];
            int companyId = (int)reader["CompanyId"];

            return new EmployeeTransferObject()
            {
                Id = id,
                Name = name,
                Surname = surname,
                Patronymic = patronymic,
                Post = post,
                HireDate = hiredate,
                CompanyId = companyId,
            };
        }
    }
}
