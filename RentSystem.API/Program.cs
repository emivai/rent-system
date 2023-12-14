using Microsoft.OpenApi.Models;
using RentSystem.Repositories.Extensions;
using RentSystem.Services.Extensions;
using Microsoft.AspNetCore.Authentication.JwtBearer;
using RentSystem.API.Extensions;
using Microsoft.AspNetCore.Authorization;
using RentSystem.Services.Handlers;
using RentSystem.API.Middlewares;

AppContext.SetSwitch("Npgsql.EnableLegacyTimestampBehavior", true);

var builder = WebApplication.CreateBuilder(args);

// Add services to the container.
builder.Services.AddRepositories(builder.Configuration);
builder.Services.AddServices(builder.Configuration);
builder.Services.AddControllers();

builder.Services
       .ConfigureAuthentication(builder.Configuration)
       .ConfigureSwagger(builder.Configuration)
       .ConfigureMvc()
       .ConfigureAuthorization();
// Learn more about configuring Swagger/OpenAPI at https://aka.ms/aspnetcore/swashbuckle

builder.Services.AddEndpointsApiExplorer();

var app = builder.Build();

// Add error handling middleware
app.UseMiddleware<ExceptionHandlingMiddleware>();

// Configure the HTTP request pipeline.

app.UseSwagger();
app.UseSwaggerUI();

app.UseHttpsRedirection();

app.UseAuthentication();
app.UseAuthorization();

app.MapControllers();

app.Run();