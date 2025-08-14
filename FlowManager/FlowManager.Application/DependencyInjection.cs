using FlowManager.Application.Interfaces;
using FlowManager.Application.IServices;
using FlowManager.Application.Services;
using FlowManager.Domain.IRepositories;
using FlowManager.Infrastructure.Services;
using FluentValidation;
using Microsoft.Extensions.DependencyInjection;

namespace FlowManager.Application;

public static class DependencyInjection
{
    public static IServiceCollection AddApplication(this IServiceCollection services)
    {
        // Add AutoMapper
        services.AddAutoMapper(typeof(DependencyInjection).Assembly);

        // Add MediatR
        services.AddMediatR(cfg => cfg.RegisterServicesFromAssembly(typeof(DependencyInjection).Assembly));

        // Add FluentValidation
        services.AddValidatorsFromAssembly(typeof(DependencyInjection).Assembly);

        services.AddScoped<IPasswordResetService, PasswordResetService>();
        services.AddScoped<IEmailService, EmailService>();
        services.AddScoped<IUserService, UserService>();
        services.AddScoped<IComponentService, ComponentService>();
        services.AddScoped<IFlowService, FlowService>();
        services.AddScoped<IFormTemplateService, FormTemplateService>();
        services.AddScoped<IRoleService, RoleService>();
        // services.AddScoped<IFormService, FormSer>();
        // services.AddScoped<IAuth, AuthService>();
        services.AddScoped<IStepService, StepService>();

        return services;
    }
}