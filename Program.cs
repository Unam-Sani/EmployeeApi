using Microsoft.EntityFrameworkCore;
using FluentValidation;
using FluentValidation.AspNetCore;

using EmployeeApi.Data;
using EmployeeApi.DTOs.Departments;
using EmployeeApi.DTOs.Employees;
using EmployeeApi.Exceptions;
using EmployeeApi.Helpers;
using EmployeeApi.Mappings;
using EmployeeApi.Repositories;
using EmployeeApi.Repositories.Interfaces;
using EmployeeApi.Services;
using EmployeeApi.Services.Interfaces;
using EmployeeApi.Validators.Departments;
using EmployeeApi.Validators.Employees;

using Microsoft.AspNetCore.Authentication.JwtBearer;
using Microsoft.IdentityModel.Tokens;

using System.Text;

var builder = WebApplication.CreateBuilder(args);


// DATABASE

builder.Services.AddDbContext<ApplicationDbContext>(options =>
    options.UseSqlite(
        builder.Configuration.GetConnectionString("DefaultConnection")));
// CONTROLLERS

builder.Services.AddControllers();


// FLUENT VALIDATION

builder.Services.AddFluentValidationAutoValidation();
builder.Services.AddFluentValidationClientsideAdapters();


// SWAGGER
// (Doesn't affect Postman. Safe to leave.)

builder.Services.AddEndpointsApiExplorer();
builder.Services.AddSwaggerGen();


// AUTOMAPPER

builder.Services.AddAutoMapper(typeof(MappingProfile));


// REPOSITORIES

builder.Services.AddScoped<IEmployeeRepository, EmployeeRepository>();
builder.Services.AddScoped<IDepartmentRepository, DepartmentRepository>();


// SERVICES

builder.Services.AddScoped<IEmployeeService, EmployeeService>();
builder.Services.AddScoped<IDepartmentService, DepartmentService>();

builder.Services.AddScoped<IAuthService, AuthService>();

builder.Services.AddSingleton<JwtTokenGenerator>();


// VALIDATORS

builder.Services.AddTransient<IValidator<DepartmentCreateDto>, DepartmentCreateValidator>();
builder.Services.AddTransient<IValidator<DepartmentUpdateDto>, DepartmentUpdateValidator>();

builder.Services.AddTransient<IValidator<EmployeeCreateDto>, EmployeeCreateValidator>();
builder.Services.AddTransient<IValidator<EmployeeUpdateDto>, EmployeeUpdateValidator>();



// JWT

var jwtSettings = builder.Configuration.GetSection("Jwt");

var jwtKey = jwtSettings["Key"]
    ?? throw new InvalidOperationException("JWT Key missing");

var key = Encoding.UTF8.GetBytes(jwtKey);

builder.Services.AddAuthentication(options =>
{
    options.DefaultAuthenticateScheme = JwtBearerDefaults.AuthenticationScheme;
    options.DefaultChallengeScheme = JwtBearerDefaults.AuthenticationScheme;
})
.AddJwtBearer(options =>
{
    options.TokenValidationParameters = new TokenValidationParameters
    {
        ValidateIssuer = true,
        ValidateAudience = true,
        ValidateLifetime = true,
        ValidateIssuerSigningKey = true,

        ValidIssuer = jwtSettings["Issuer"],
        ValidAudience = jwtSettings["Audience"],

        IssuerSigningKey = new SymmetricSecurityKey(key)
    };
});



// CORS

builder.Services.AddCors(options =>
{
    options.AddPolicy("AllowFrontend", policy =>
    {
        policy.AllowAnyOrigin()
              .AllowAnyHeader()
              .AllowAnyMethod();
    });
});


// BUILD APPLICATION

var app = builder.Build();



using (var scope = app.Services.CreateScope())
{
    var services = scope.ServiceProvider;
    var logger = services.GetRequiredService<ILogger<Program>>();

    try
    {
        var db = services.GetRequiredService<ApplicationDbContext>();

        logger.LogInformation("Applying Entity Framework migrations...");

        db.Database.Migrate();

        logger.LogInformation("Database migrations applied successfully.");
    }
    catch (Exception ex)
    {
        logger.LogError(ex, "An error occurred while applying database migrations.");
        throw;
    }
}
// MIDDLEWARE

app.UseMiddleware<GlobalExceptionMiddleware>();

app.UseCors("AllowFrontend");

if (app.Environment.IsDevelopment())
{
    app.UseSwagger();
    app.UseSwaggerUI();
}

app.UseHttpsRedirection();

app.UseAuthentication();

app.UseAuthorization();


// ENDPOINTS

app.MapControllers();

app.Run();

