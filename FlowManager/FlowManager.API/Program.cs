﻿using FlowManager.API;
using FlowManager.Application;
using FlowManager.Application.Interfaces;
using FlowManager.Application.IServices;
using FlowManager.Application.Services;
using FlowManager.Application.Utils;
using FlowManager.Application.Utils;
using FlowManager.Domain.Entities;
using FlowManager.Domain.IRepositories;
using FlowManager.Infrastructure;
using FlowManager.Infrastructure.Context;
using FlowManager.Infrastructure.Context;
using FlowManager.Infrastructure.Middleware;
using FlowManager.Infrastructure.Repositories;
using FlowManager.Infrastructure.Seed;
using FlowManager.Infrastructure.Services;
using FlowManager.Infrastructure.Utils;
using FlowManager.Infrastructure.Utils;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Identity.EntityFrameworkCore;
using Microsoft.AspNetCore.Identity.EntityFrameworkCore;
using Microsoft.AspNetCore.Routing;
using Microsoft.Extensions.Options;

var builder = WebApplication.CreateBuilder(args);

// CORS
builder.Services.AddCors(options =>
{
    options.AddPolicy("CorsPolicy", policy =>
    {
        policy.WithOrigins("https://localhost:7082", "http://localhost:5223", "https://localhost:7195") 
              .AllowAnyHeader()
              .AllowAnyMethod()
              .AllowCredentials(); 
    });
});

builder.Services.ConfigureApplicationCookie(options =>
{
    options.Cookie.Name = "FlowManagerAuth";
    options.LoginPath = "/api/auth/login";
    options.AccessDeniedPath = "/access-denied";
    options.Cookie.SameSite = SameSiteMode.None;
    options.Cookie.SecurePolicy = CookieSecurePolicy.Always;
    options.Cookie.HttpOnly = false;
    options.Events.OnRedirectToLogin = context =>
    {
        context.Response.StatusCode = 401;
        return Task.CompletedTask;
    };
    options.Events.OnRedirectToAccessDenied = context =>
    {
        context.Response.StatusCode = 403;
        return Task.CompletedTask;
    };
});


// Add layer dependencies
builder.Services.AddInfrastructure(builder.Configuration);
builder.Services.AddApplication();
builder.Services.AddEndpointsApiExplorer();
builder.Services.AddSwaggerGen();
builder.Services.AddControllers()
    .AddJsonOptions(options =>
    {
        options.JsonSerializerOptions.ReferenceHandler = System.Text.Json.Serialization.ReferenceHandler.IgnoreCycles;
        options.JsonSerializerOptions.DefaultIgnoreCondition = System.Text.Json.Serialization.JsonIgnoreCondition.WhenWritingNull;
    });

// Learn more about configuring OpenAPI at https://aka.ms/aspnet/openapi
builder.Services.AddOpenApi();

builder.Services.AddAuthorization();
builder.Services.AddHttpContextAccessor();

builder.Services.AddIdentityCore<User>()
    .AddRoles<Role>()
    .AddEntityFrameworkStores<AppDbContext>()
    .AddSignInManager()
    .AddRoleManager<RoleManager<Role>>()
    .AddDefaultTokenProviders();

builder.Services.AddScoped<IFormResponseRepository, FormResponseRepository>();


builder.Services.AddSingleton<IEmailSender<User>, NoOpEmailSender>();

var app = builder.Build();

using (var scope = app.Services.CreateScope())
{
    AppDbContext dbContext = scope.ServiceProvider.GetRequiredService<AppDbContext>();
    IPasswordHasher<User> passwordHasher = scope.ServiceProvider.GetRequiredService<IPasswordHasher<User>>();

    //BasicSeed.Populate(dbContext, passwordHasher);
   // MockDataSeed.Populate(dbContext, passwordHasher);
}

if (app.Environment.IsDevelopment())
{
    app.MapOpenApi();
    app.UseSwagger();
    app.UseSwaggerUI();
}

app.UseHttpsRedirection();

// Use CORS
app.UseRouting();
app.UseCors("CorsPolicy");

app.UseAuthentication();
app.UseAuthorization();

app.MapIdentityApi<User>();

app.UseMiddleware<ExceptionHandlingMiddleware>();

app.MapControllers();

app.Run();
