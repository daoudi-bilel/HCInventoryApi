using Microsoft.EntityFrameworkCore;
using System.Reflection;
using Microsoft.OpenApi.Models;
using ITInventoryManagementAPI.Services;
using System.Text.Json.Serialization;

var builder = WebApplication.CreateBuilder(args);

// Add services to the container.
builder.Services.AddEndpointsApiExplorer();

builder.Services.AddCors(options =>
{
   options.AddDefaultPolicy(builder =>
    {
        builder.WithOrigins("http://localhost:4200","https://hcinventory.netlify.app")
               .WithMethods("GET", "POST", "PUT", "DELETE", "PATCH")
               .WithHeaders("Content-Type", "Authorization");
    });
});

builder.Services.AddSwaggerGen(options =>
{
    options.SwaggerDoc("v1", new OpenApiInfo
    {
        Version = "v1",
        Title = "HCInventory API",
        Description = "It inventory management api",
        TermsOfService = new Uri("https://bilos.netlify.app"),
        Contact = new OpenApiContact
        {
            Name = "Daoudi Bilel",
            Email = "daoudi-bilel@outlook.com"
        },
    });
});

builder.Services.AddScoped<IEmployeeService, EmployeeService>();
builder.Services.AddScoped<IDeviceService, DeviceService>();


builder.Services.AddControllers().AddJsonOptions(options =>
{
    options.JsonSerializerOptions.ReferenceHandler = ReferenceHandler.IgnoreCycles;
});

builder.Services.AddDbContext<ITInventoryContext>(options =>
    options.UseSqlServer(builder.Configuration.GetConnectionString("DefaultConnection")));


builder.Logging.AddConsole();

var app = builder.Build();

if (app.Environment.IsDevelopment())
{
    app.UseDeveloperExceptionPage();
    
}

    app.UseSwagger();
    app.UseSwaggerUI();

app.UseHttpsRedirection();

app.MapControllers();

app.UseCors();

app.Run();