using Microsoft.AspNetCore.Identity;

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
        if (!await roleManager.RoleExistsAsync("Client"))
            await roleManager.CreateAsync(new IdentityRole("Client"));

        // 2. Create admin user if not exists
        string adminEmail = configurations["AdminCredentials:Email"]!;
        string Password = configurations["AdminCredentials:Password"]!;
        string Username = configurations["AdminCredentials:Name"]!;
        string Phone = configurations["AdminCredentials:Phone"]!;
        Employee emp = dbContext.Employees.FirstOrDefault(e => e.PhoneNum == Phone)!;
        Person? adminUser = null;
        //  = await userManager.FindByEmailAsync(adminEmail);
        if (emp == null)
        {
            adminUser = new Person
            {
                UserName = adminEmail,
                Email = adminEmail,
                EmailConfirmed = true
            };

            await userManager.CreateAsync(adminUser, Password);
            await userManager.AddToRoleAsync(adminUser, "Employee");
        }

        // 3. Seed "AccessAdminPanel" privilege if not exists
        var adminPrivilege = dbContext.Privileges.FirstOrDefault(p => p.Name == "Admin");
        if(adminPrivilege == null) {
            return;
        }

        // 4. Add to Employees table if not already
        var employee = dbContext.Employees.FirstOrDefault(e => e.Name == Username);
        if (employee == null) {
            employee = new Employee {
                Name = Username,
                EmployeeType = EmployeeType.Manager,
                Balance = 0,
                PhoneNum = Phone,
                Privileges = adminPrivilege,
                Person = adminUser!
            };
            dbContext.Employees.Add(employee);
            await dbContext.SaveChangesAsync();
        }
        //5. Add to Clients table if not already

        Client c1 = dbContext.Clients.FirstOrDefault(e => e.NickName == configurations["VClient:NickName"])!;
        if (c1 == null)
        {
            var VClientPerson = new Person
            {
                UserName = configurations["VClient:Email"]!,
                Email = configurations["VClient:Email"]!,
                EmailConfirmed = true
            };
            await userManager.CreateAsync(VClientPerson, configurations["VClient:Password"]!);
            await userManager.AddToRoleAsync(VClientPerson, "Client");
            var VClient = await userManager.FindByEmailAsync(adminEmail);
            Client c = new()
            {
                Name = configurations["VClient:Name"]!,
                PhoneNumber = configurations["VClient:Phone"]!,
                NickName = configurations["VClient:NickName"]!,
                Person = VClientPerson
            };
            dbContext.Clients.Add(c);
            dbContext.SaveChanges();
        }
    }
}
