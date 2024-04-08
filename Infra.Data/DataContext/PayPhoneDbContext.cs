using Microsoft.AspNetCore.Identity.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore;
using Models;

namespace Infra.Data.DataContext
{
    public class PayPhoneDbContext : IdentityDbContext
    {
        public PayPhoneDbContext(DbContextOptions<PayPhoneDbContext> options)
            : base(options) { }

        public DbSet<Employee> Employees { get; set; }
    }
}
