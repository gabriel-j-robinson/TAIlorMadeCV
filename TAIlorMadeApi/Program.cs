using TAIlorMadeApi;
using TAIlorMadeApi.Models;
using TAIlorMadeApi.Jobs;
using Microsoft.EntityFrameworkCore;
using Hangfire;
using Hangfire.MemoryStorage;

var builder = WebApplication.CreateBuilder(args);

// Add Hangfire services
builder.Services.AddHangfire(config =>
{
    config.UseMemoryStorage();
});
builder.Services.AddHangfireServer();

builder.Services.AddControllers();
builder.Services.AddDbContext<ResumeRequestContext>(opt =>
    opt.UseInMemoryDatabase("ResumeRequestDB"));
builder.Services.AddScoped<CoverLetterJob>();


builder.Services.AddEndpointsApiExplorer();
builder.Services.AddSwaggerGen(c =>
{
    c.OperationFilter<SwaggerFileUploadOperationFilter>();
});
builder.Services.AddCors();

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
