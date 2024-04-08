namespace Models
{
    public interface IEmployeesContract
    {
        Task<List<Employee>> GetEmployeesAsync();
        Employee GetEmployeeByIdAsync(int id);
        Task<int> CreateEmployeeAsync(Employee employee);
        Task UpdateEmployeeAsync(Employee employee);
        Task<int> DeleteEmployeeAsync(Employee employee);
    }
}
