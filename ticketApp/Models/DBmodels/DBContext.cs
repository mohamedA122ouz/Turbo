using System;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Identity.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore;
using ticketApp.Models.DBmodels;

namespace ticketApp.Models.Dbmodels;

public class DBContext : IdentityDbContext<Person>
{
    public DBContext(DbContextOptions<DBContext> options) : base(options)
    { }
    public DbSet<Employee> Employees { get; set; }
    public DbSet<Broker> Brokers { get; set; }
    public DbSet<Ticket> Tickets { get; set; }
    public DbSet<Payment> Payments { get; set; }
    public DbSet<Client> Clients { get; set; }
    public DbSet<IssueCompany> IssueCompanies { get; set; }
    public DbSet<Privileges> Privileges { get; set; }
    public DbSet<Balance> Balance {get;set;}
    public DbSet<ReIssuedTickets> ReIssuedTickets { get; set; }

    protected override void OnModelCreating(ModelBuilder builder)
    {
        builder.Entity<Employee>().HasMany(e => e.Prokers).WithOne(p => p.Employee).OnDelete(DeleteBehavior.Cascade);
        builder.Entity<Employee>().HasMany(e => e.Tickets).WithOne(t => t.Employee).OnDelete(DeleteBehavior.Restrict);
        builder.Entity<Broker>().HasMany(e => e.Tickets).WithOne(t => t.Broker).OnDelete(DeleteBehavior.Cascade);
        builder.Entity<Client>().HasMany(c => c.Ticket).WithOne(t => t.Client).OnDelete(DeleteBehavior.Restrict);
        builder.Entity<Ticket>().HasMany(t => t.Payments).WithOne(p => p.Ticket);
        builder.Entity<Privileges>().HasMany(p => p.Employees).WithOne(e => e.Privileges).OnDelete(DeleteBehavior.Cascade);
        builder.Entity<IssueCompany>().HasMany(i => i.Tickets).WithOne(t => t.IssueCompany).OnDelete(DeleteBehavior.Cascade);
        builder.Entity<Person>().HasOne(p => p.Employee).WithOne(e => e.Person).HasForeignKey<Employee>(e=>e.personId);
        builder.Entity<Person>().HasOne(p=>p.client).WithOne(e => e.Person).HasForeignKey<Client>(c=>c.personId);
        builder.Entity<ReIssuedTickets>().HasOne(t => t.NewTicket).WithOne().OnDelete(DeleteBehavior.Cascade);
        base.OnModelCreating(builder);
        builder.Entity<Privileges>().HasData(
            new Privileges
            {
                Id = 1,
                Name = "Admin",
                Description = "Administrator with full access to the system.",
                CanCreateTicket = true,
                CanViewTicket = true,
                CanEditTicket = true,
                CanDeleteTicket = true,
                CanCreateBroker = true,
                CanViewBroker = true,
                CanEditBroker = true,
                CanDeleteBroker = true,
                CanShowAnalytics = true,
                CanViewPayments = true,
                CanEditPayments = true,
                CanDeletePayments = true,
                CanCreatePayment = true,
                CanViewClients = true,
                CanEditClients = true,
                CanDeleteClients = true,
                CanViewEmployees = true,
                CanEditEmployees = true,
                CanDeleteEmployees = true,
                CanViewPrivileges = true,
                CanEditPrivileges = true,
                CanDeletePrivileges = true,
                CanViewSettings = true,
                CanEditSettings = true,
                CanDeleteSettings = true,
                Employees = new List<Employee>()
            });
          
/*        builder.Entity<Employee>().HasData(
            new Employee
            {
                Name = "admin",
                PhoneNum = "1234567890",
                Email = "test@gmail.com",
                EmployeeType = Utility.EmployeeType.Manager,
                PrivilegesId = 1,
                EmailConfirmed = true,
                PasswordHash = new PasswordHasher<Employee>().HashPassword(null, "Admin@123"),
                SecurityStamp = Guid.NewGuid().ToString(),
                Balance = 0,
                Salary = 100000,
                ImagePath = "https://cdn.pixabay.com/photo/2019/05/20/00/55/power-4215692_960_720.jpg",
            });*/
    }
}
