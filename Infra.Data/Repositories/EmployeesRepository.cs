using Infra.Data.DataContext;
using Microsoft.EntityFrameworkCore;
using Models;

namespace Infra.Data.Repositories
{
    public class EmployeesRepository : IEmployeesContract
    {
        private readonly PayPhoneDbContext _context;
        public EmployeesRepository(PayPhoneDbContext context) 
            => this._context = context;

        public async Task<List<Employee>> GetEmployeesAsync() 
            => await this._context.Employees.ToListAsync();

        public Employee GetEmployeeByIdAsync(int id)
        {
            if (id == 0)
                return null;

            var employee = this._context.Employees.FirstOrDefault(x => x.Id == id);
            
            return employee;
        }

        public async Task<int> CreateEmployeeAsync(Employee employee)
        {
            if (employee == null) 
                throw new ArgumentNullException(nameof(employee));

            await this._context.Employees.AddAsync(employee);
            return await this._context.SaveChangesAsync();
        }
        
        public async Task UpdateEmployeeAsync(Employee employee)
        {
            if(employee == null) 
                throw new ArgumentNullException(nameof(employee));

            this._context.Update(employee);
            await this._context.SaveChangesAsync();
        }

        public async Task<int> DeleteEmployeeAsync(Employee employee)
        {
            this._context.Employees.Remove(employee);

            return await this._context.SaveChangesAsync();
        }
    }
}
