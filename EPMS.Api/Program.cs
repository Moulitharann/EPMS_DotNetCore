using EmployeePerformancesManagementSystem.Core.Services;
using EmployeePerformancesManagementSystem.MVC;
using EmployeePerformancesManagementSystem.MVC.Services;
using EmployeePerformancesManagementSystem.MVC.DataService;
using Microsoft.EntityFrameworkCore;
using EmployeePerformancesManagementSystem.MVC.Data;
using EPMS.Data;

var builder = WebApplication.CreateBuilder(args);

builder.Logging.ClearProviders();
builder.Logging.AddConsole();

builder.Services.AddDbContext<DBConnection>(options =>
    options.UseSqlServer(builder.Configuration.GetConnectionString("DefaultConnection")));

builder.Services.AddScoped<ApplicationDbService>();
builder.Services.AddScoped<AuthService>();
builder.Services.AddScoped<AssignManagerService>();
builder.Services.AddScoped<AssignManagerDataService>();
builder.Services.AddScoped<EmployeeDashboardService>();
builder.Services.AddScoped<EmployeeDashboardDataService>();
builder.Services.AddScoped<UserService>();
builder.Services.AddScoped<UserRepository>();
builder.Services.AddScoped<ManagerService>();
builder.Services.AddScoped<ManagerDataService>();

// Authentication & Authorization
builder.Services.AddAuthentication();
builder.Services.AddAuthorization();

builder.Services.AddControllers();
builder.Services.AddEndpointsApiExplorer();
builder.Services.AddSwaggerGen();

builder.Services.AddCors(options =>
{
    options.AddPolicy("AllowFrontend", policy =>
    {
        policy.WithOrigins("http://localhost:5087")
              .AllowAnyHeader()
              .AllowAnyMethod()
              .AllowCredentials();
    });
});

var app = builder.Build();

if (app.Environment.IsDevelopment())
{
    using (var scope = app.Services.CreateScope())
    {
        var dbContext = scope.ServiceProvider.GetRequiredService<DBConnection>();
        try
        {
            Console.WriteLine("🔄 Testing database connection...");
            dbContext.Database.EnsureCreated();
            Console.WriteLine("✅ Database is ready! 🎉");
        }
        catch (Exception ex)
        {
            Console.WriteLine($"❌ Database connection failed: {ex.Message}");
        }
    }
}

app.UseHttpsRedirection();
app.UseCors("AllowFrontend");
app.UseAuthentication();
app.UseAuthorization();

if (app.Environment.IsDevelopment())
{
    app.UseSwagger();
    app.UseSwaggerUI();
}

app.MapControllers();

app.Run();
