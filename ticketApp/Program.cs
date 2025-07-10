using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore;
using ticketApp.Models.Dbmodels;
using ticketApp.Models.DBmodels;
using ticketApp.Services;

var builder = WebApplication.CreateBuilder(args);

// Add services to the container.
builder.Services.AddControllersWithViews();
var connectionString = builder.Configuration.GetConnectionString("Default");
builder.Services.AddDefaultIdentity<Person>(option => option.SignIn.RequireConfirmedAccount = true)
.AddRoles<IdentityRole>()
.AddEntityFrameworkStores<DBContext>();
builder.Services.AddDbContext<DBContext>(options=>
{
    options.UseSqlServer(connectionString);
});
builder.Services.AddScoped<UserServices>();
builder.Services.AddScoped<TicketEngine>();
builder.Logging.ClearProviders();
builder.Logging.AddConsole();
var app = builder.Build();

// Configure the HTTP request pipeline.
if (!app.Environment.IsDevelopment())
{
    app.UseExceptionHandler("/Home/Error");
    // The default HSTS value is 30 days. You may want to change this for production scenarios, see https://aka.ms/aspnetcore-hsts.
    app.UseHsts();
}

app.UseHttpsRedirection();
app.UseStaticFiles();

app.UseRouting();

app.UseAuthentication();
app.UseAuthorization();
app.MapRazorPages();
app.MapControllerRoute(
    name: "default",
    pattern: "{controller=Home}/{action=Index}/{id?}");

app.Run();
