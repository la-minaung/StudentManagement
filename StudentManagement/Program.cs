using Microsoft.EntityFrameworkCore;
using Serilog;
using StudentManagement.Data;
using StudentManagement.Filters;
using StudentManagement.Middlewares;
using StudentManagement.Services;

var builder = WebApplication.CreateBuilder(args);

builder.Host.UseSerilog((context, configuration) => configuration.ReadFrom.Configuration(context.Configuration));

// Add services to the container.

builder.Services.AddControllers(options =>
{
    options.Filters.Add<ResponseWrapperFilter>();
});

builder.Services.AddDbContext<ApplicationDbContext>(options =>
    options.UseSqlServer(builder.Configuration.GetConnectionString("DefaultConnection")));

builder.Services.AddScoped<IStudentService, StudentService>();
builder.Services.AddScoped<IDepartmentService, DepartmentService>();
builder.Services.AddScoped<ITeacherService, TeacherService>();

// Learn more about configuring Swagger/OpenAPI at https://aka.ms/aspnetcore/swashbuckle
builder.Services.AddEndpointsApiExplorer();
builder.Services.AddSwaggerGen();

var app = builder.Build();

// Custom middleware example - logs before and after each request
app.Use(async (context, next) =>
{
    var logger = context.RequestServices.GetRequiredService<ILogger<Program>>();

    // BEFORE: Code here runs before the request reaches the controller
    logger.LogInformation("[Before] Request: {Method} {Path}", context.Request.Method, context.Request.Path);

    await next();  // Call the next middleware in the pipeline

    // AFTER: Code here runs after the response is generated
    logger.LogInformation("[After] Response: {StatusCode}", context.Response.StatusCode);
});

// Configure the HTTP request pipeline.
if (app.Environment.IsDevelopment())
{
    app.UseSwagger();
    app.UseSwaggerUI();
}

app.UseMiddleware<GlobalExceptionMiddleware>();

app.UseHttpsRedirection();

app.UseAuthorization();

app.MapControllers();

app.Run();
