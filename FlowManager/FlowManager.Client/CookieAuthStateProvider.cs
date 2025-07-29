using Microsoft.AspNetCore.Components.Authorization;
using System.Net.Http.Json;
using System.Security.Claims;
using FlowManager.Domain.Entities;
using Microsoft.AspNetCore.Components.WebAssembly.Http;

public class CookieAuthStateProvider : AuthenticationStateProvider
{
    private readonly HttpClient _httpClient;
    private string _email;

    public CookieAuthStateProvider(HttpClient httpClient)
    {
        _httpClient = httpClient;
    }
    public void SetEmail(string email)
    {
        _email = email;
    }
    public override async Task<AuthenticationState> GetAuthenticationStateAsync()
    {
        try
        {
            var request = new HttpRequestMessage(HttpMethod.Get, "/api/Users/me");
            request.SetBrowserRequestCredentials(BrowserRequestCredentials.Include);
            var response = await _httpClient.SendAsync(request);
            
            if (!response.IsSuccessStatusCode)
            {
                Console.WriteLine("0");
                return new AuthenticationState(new ClaimsPrincipal(new ClaimsIdentity()));
            }
                

            var user = await response.Content.ReadFromJsonAsync<User>();

            if (user == null || string.IsNullOrWhiteSpace(user.Email))
            {
                Console.WriteLine("1");
                return new AuthenticationState(new ClaimsPrincipal(new ClaimsIdentity()));
            }

            var identity = new ClaimsIdentity(new[]
            {
                new Claim(ClaimTypes.Name, string.IsNullOrWhiteSpace(user.Name) ? user.Email : user.Name),
                new Claim(ClaimTypes.Email, user.Email),
                //new Claim(ClaimTypes.Role, user.UserRoles)
            }, "Cookies");

            var principal = new ClaimsPrincipal(identity);
            Console.WriteLine("OK");
            return new AuthenticationState(principal);
        }
        catch (Exception ex)
        {
            Console.WriteLine($"[AuthState] Eroare la GetAuthenticationStateAsync: {ex.Message}");
            return new AuthenticationState(new ClaimsPrincipal(new ClaimsIdentity()));
        }
    }

    public void NotifyUserAuthentication(string _email)
    {
        SetEmail(_email);
        NotifyAuthenticationStateChanged(GetAuthenticationStateAsync());
    }

    public void NotifyUserLogout()
    {
        var anonymous = new AuthenticationState(new ClaimsPrincipal(new ClaimsIdentity()));
        NotifyAuthenticationStateChanged(Task.FromResult(anonymous));
    }
}