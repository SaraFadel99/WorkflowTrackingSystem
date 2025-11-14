using Microsoft.EntityFrameworkCore;
using WorkflowTrackingSystem.Business.Services;
using WorkflowTrackingSystem.Data;
using WorkflowTrackingSystem.Data.Repositories;

var builder = WebApplication.CreateBuilder(args);

// Add services to the container.

builder.Services.AddControllers();
// Learn more about configuring Swagger/OpenAPI at https://aka.ms/aspnetcore/swashbuckle
builder.Services.AddEndpointsApiExplorer();
builder.Services.AddSwaggerGen();

// Configure Entity Framework Core
// Using In-Memory database for development. 
// To switch to SQL Server, uncomment the SQL Server line and comment the In-Memory line
builder.Services.AddDbContext<ApplicationDbContext>(options =>
{
    // Option 1: In-Memory Database (for development/testing)
    options.UseInMemoryDatabase("WorkflowTrackingDb");
    
    // Option 2: SQL Server (uncomment to use SQL Server instead)
    // var connectionString = builder.Configuration.GetConnectionString("DefaultConnection");
    // options.UseSqlServer(connectionString);
});

// Register Repositories
builder.Services.AddScoped<IWorkflowRepository, WorkflowRepository>();

// Register Services
builder.Services.AddScoped<IWorkflowService, WorkflowService>();

var app = builder.Build();

// Configure the HTTP request pipeline.
if (app.Environment.IsDevelopment())
{
    app.UseSwagger();
    app.UseSwaggerUI();
}

app.UseHttpsRedirection();

app.UseAuthorization();

app.MapControllers();

app.Run();
