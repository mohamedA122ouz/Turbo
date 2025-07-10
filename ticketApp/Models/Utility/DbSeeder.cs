using Microsoft.AspNetCore.Identity;
using ticketApp.Models.Dbmodels;
using ticketApp.Models.DBmodels;
using ticketApp.Models.Utility;

public static class DbSeeder {
    public static async Task SeedAdminAsync(IServiceProvider serviceProvider) {
        var roleManager = serviceProvider.GetRequiredService<RoleManager<IdentityRole>>();
        var userManager = serviceProvider.GetRequiredService<UserManager<Person>>();
        var dbContext = serviceProvider.GetRequiredService<DBContext>();
        var configurations = serviceProvider.GetRequiredService<IConfiguration>();

        // 1. Create "Employee" Role if not exists
        if (!await roleManager.RoleExistsAsync("Employee"))
            await roleManager.CreateAsync(new IdentityRole("Employee"));

        // 2. Create admin user if not exists
        string adminEmail = configurations["AdminCredentials:Email"]!;
        string Password = configurations["AdminCredentials:Password"]!;
        string Username = configurations["AdminCredentials:Name"]!;
        string Phone = configurations["AdminCredentials:Phone"]!;
        var adminUser = await userManager.FindByEmailAsync(adminEmail);
        if (adminUser == null) {
            adminUser = new Person {
                UserName = Username,
                Email = adminEmail,
                EmailConfirmed = true
            };

            await userManager.CreateAsync(adminUser, Password); // Secure this in real app
            await userManager.AddToRoleAsync(adminUser, "Employee");
        }

        // 3. Seed "AccessAdminPanel" privilege if not exists
        var adminPrivilege = dbContext.Privileges.FirstOrDefault(p => p.Name == "Admin");
        if(adminPrivilege == null) {
            return;
        }

        // 4. Add to Employees table if not already
        var employee = dbContext.Employees.FirstOrDefault(e => e.Id == 1);
        if (employee == null) {
            employee = new Employee {
                Id = 1,
                Name = Username,
                EmployeeType = EmployeeType.Manager,
                Balance = 0,
                PhoneNum = Phone,
                Privileges = adminPrivilege
            };
            dbContext.Employees.Add(employee);
            await dbContext.SaveChangesAsync();
        }

    }
}
