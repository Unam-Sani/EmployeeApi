using Microsoft.EntityFrameworkCore;
using FluentValidation;
using FluentValidation.AspNetCore;

using EmployeeApi.Data;
using EmployeeApi.DTOs.Departments;
using EmployeeApi.DTOs.Employees;
using EmployeeApi.Exceptions;
using EmployeeApi.Mappings;
using EmployeeApi.Repositories;
using EmployeeApi.Repositories.Interfaces;
using EmployeeApi.Services;
using EmployeeApi.Services.Interfaces;
using EmployeeApi.Validators.Departments;
using EmployeeApi.Validators.Employees;

using Microsoft.AspNetCore.Authentication.JwtBearer;
using Microsoft.IdentityModel.Tokens;
using EmployeeApi.Helpers;


using System.Text;


var builder = WebApplication.CreateBuilder(args);


// Database
builder.Services.AddDbContext<ApplicationDbContext>(options =>
    options.UseSqlServer(
        builder.Configuration.GetConnectionString("DefaultConnection")));



// Controllers
builder.Services.AddControllers();


// FluentValidation
builder.Services.AddFluentValidationAutoValidation();
builder.Services.AddFluentValidationClientsideAdapters();


// Swagger
builder.Services.AddEndpointsApiExplorer();
builder.Services.AddSwaggerGen();


// AutoMapper
builder.Services.AddAutoMapper(typeof(MappingProfile));


// Repositories
builder.Services.AddScoped<IEmployeeRepository, EmployeeRepository>();
builder.Services.AddScoped<IDepartmentRepository, DepartmentRepository>();


// Services
builder.Services.AddScoped<IEmployeeService, EmployeeService>();
builder.Services.AddScoped<IDepartmentService, DepartmentService>();


// Validators
builder.Services.AddTransient<IValidator<DepartmentCreateDto>, DepartmentCreateValidator>();
builder.Services.AddTransient<IValidator<DepartmentUpdateDto>, DepartmentUpdateValidator>();

builder.Services.AddTransient<IValidator<EmployeeCreateDto>, EmployeeCreateValidator>();
builder.Services.AddTransient<IValidator<EmployeeUpdateDto>, EmployeeUpdateValidator>();

//Register AuthService and JwtTokenGenerator
builder.Services.AddScoped<IAuthService, AuthService>();
builder.Services.AddSingleton<JwtTokenGenerator>();


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

builder.Services.AddCors(options =>
{
    options.AddPolicy("AllowFrontend",
        policy =>
        {
            policy.AllowAnyOrigin()
                  .AllowAnyHeader()
                  .AllowAnyMethod();
        });
});

var app = builder.Build();

app.UseCors("AllowFrontend");

// Middleware
app.UseMiddleware<GlobalExceptionMiddleware>();

if (app.Environment.IsDevelopment())
{
    app.UseSwagger();
    app.UseSwaggerUI();
}

app.UseHttpsRedirection();

app.UseAuthentication();

app.UseAuthorization();


// Endpoints
app.MapControllers();

app.Run();

