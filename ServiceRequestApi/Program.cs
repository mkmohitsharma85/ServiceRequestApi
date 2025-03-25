using AutoMapper;
using Microsoft.EntityFrameworkCore;
using ServiceRequest.BAL;
using ServiceRequest.BAL.IServiceRequest.BAL;
using ServiceRequest.BAL.ServiceRequest.BAL;
using ServiceRequest.DAL.EntityModel;
using ServiceRequest.DAL.IServiceRequest;
using ServiceRequest.DAL.ServiceRequestRepo;
using ServiceRequestApi;

var builder = WebApplication.CreateBuilder(args);

builder.Services.AddAutoMapper(AppDomain.CurrentDomain.GetAssemblies());
// Add services to the container.
builder.Services.AddDbContext<ApplicationDBContext>(options =>
            options.UseSqlServer(builder.Configuration.GetConnectionString("DefaultConnection")));

builder.Services.AddScoped<IServiceRequestRepo, ServiceRequestRepo>();
builder.Services.AddScoped<IServiceRequestService, ServiceRequestService>();
builder.Services.AddTransient<IEmailService, EmailService>();

builder.Services.AddControllers();
// Learn more about configuring Swagger/OpenAPI at https://aka.ms/aspnetcore/swashbuckle
builder.Services.AddEndpointsApiExplorer();
builder.Services.AddSwaggerGen();

var app = builder.Build();

// Configure the HTTP request pipeline.
if (app.Environment.IsDevelopment())
{
    app.UseSwagger();
    app.UseSwaggerUI();
}

app.UseHttpsRedirection();

app.UseAuthorization();
app.UseMiddleware<GlobalExceptionMiddleware>();
app.MapControllers();

app.Run();
