using Microsoft.AspNetCore.Components.Web;
using Microsoft.AspNetCore.Components.WebAssembly.Hosting;
using FlowManager.Client;
using FlowManager.Client.Services;
using Microsoft.AspNetCore.Components.Authorization;

var builder = WebAssemblyHostBuilder.CreateDefault(args);

builder.RootComponents.Add<App>("#app");
builder.RootComponents.Add<HeadOutlet>("head::after");

builder.Services.AddAuthorizationCore();
builder.Services.AddScoped<CookieAuthStateProvider>();
builder.Services.AddScoped<AuthenticationStateProvider>(provider =>
    provider.GetRequiredService<CookieAuthStateProvider>());

builder.Services.AddCascadingAuthenticationState();

builder.Services.AddApiAuthorization();

builder.Services.AddScoped(sp => 
{
    var httpClient = new HttpClient()
    {
        BaseAddress = new Uri("https://localhost:5000/")
    };
    
    // Ensure credentials (cookies) are sent with requests
    httpClient.DefaultRequestHeaders.Add("X-Requested-With", "XMLHttpRequest");
    
    return httpClient;
});

// Register services
builder.Services.AddScoped<FlowService>();
builder.Services.AddScoped<StepService>();
builder.Services.AddScoped<UserService>();
builder.Services.AddScoped<FormService>();
builder.Services.AddScoped<FormTemplateService>();
builder.Services.AddScoped<ComponentService>();
builder.Services.AddScoped<RoleService>();
builder.Services.AddScoped<AuthService>();
builder.Services.AddScoped<PasswordResetService>();



builder.Services.AddBlazorBootstrap();

await builder.Build().RunAsync();