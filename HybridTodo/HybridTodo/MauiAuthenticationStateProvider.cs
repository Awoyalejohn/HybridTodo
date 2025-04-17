using HybridTodo.Abstractions.Services;
using HybridTodo.Shared.DTOs;
using Microsoft.AspNetCore.Authentication;
using Microsoft.AspNetCore.Authentication.BearerToken;
using Microsoft.AspNetCore.Components.Authorization;
using Microsoft.AspNetCore.DataProtection;
using Microsoft.AspNetCore.Http;
using System.Diagnostics;
using System.Net.Http.Json;
using System.Security.Claims;

namespace HybridTodo;

/// <summary>
/// This class manages the authentication state of the user.
/// The class handles user login, logout, and token validation, including refreshing tokens when they are close to expiration.
/// It uses secure storage to save and retrieve tokens, ensuring that users do not need to log in every time.
/// </summary>
internal sealed class MauiAuthenticationStateProvider : AuthenticationStateProvider
{
    private readonly ITokenStorageService _tokenStorageService;
    private readonly HttpClient _httpClient;
    private readonly IDataProtectionProvider _dataProtectionProvider;

    private const int TokenExpirationBuffer = 30; //minutes

    private static readonly ClaimsPrincipal _defaultUser = new ClaimsPrincipal(new ClaimsIdentity());
    private static readonly Task<AuthenticationState> _defaultAuthState = Task.FromResult(new AuthenticationState(_defaultUser));

    private Task<AuthenticationState> _currentAuthState = _defaultAuthState;
    private AccessTokenInfo? _accessToken;

    public MauiAuthenticationStateProvider(ITokenStorageService tokenStorageService, HttpClient httpClient, IDataProtectionProvider dataProtectionProvider)
    {
        _tokenStorageService = tokenStorageService;
        _httpClient = httpClient;
        _dataProtectionProvider = dataProtectionProvider;
    }

    public override Task<AuthenticationState> GetAuthenticationStateAsync()
    {
        if (_currentAuthState != _defaultAuthState)
        {
            return _currentAuthState;
        }

        _currentAuthState = CreateAuthenticationStateFromSecureStorageAsync();
        NotifyAuthenticationStateChanged(_currentAuthState);

        return _currentAuthState;
    }

    public async Task<AccessTokenInfo?> GetAccessTokenInfoAsync()
    {
        if (await UpdateAndValidateAccessTokenAsync())
        {
            return _accessToken;
        }

        Logout();
        return null;
    }

    public void Logout()
    {
        _currentAuthState = _defaultAuthState;
        _accessToken = null;
        _tokenStorageService.RemoveToken();
        NotifyAuthenticationStateChanged(_defaultAuthState);
    }

    public Task<AuthenticationState> LogInAsync(LoginResponse loginModel)
    {
        _currentAuthState = LogInAsyncCore(loginModel);
        NotifyAuthenticationStateChanged(_currentAuthState);

        return _currentAuthState;

        async Task<AuthenticationState> LogInAsyncCore(LoginResponse loginModel)
        {
            var user = await LoginWithProviderAsync(loginModel);
            return new AuthenticationState(user);
        }
    }

    private async Task<ClaimsPrincipal> LoginWithProviderAsync(LoginResponse loginModel)
    {
        var authenticatedUser = _defaultUser;

        try
        {
            // Save token to secure storage so the user doesn't have to login every time
            _accessToken = await _tokenStorageService.SaveTokenToSecureStorageAsync(loginModel);

            authenticatedUser = CreateAuthenticatedUser(loginModel.AccessToken);
        }
        catch (Exception ex)
        {
            Debug.WriteLine($"Error logging in: {ex}");
        }

        return authenticatedUser;
    }

    private async Task<AuthenticationState> CreateAuthenticationStateFromSecureStorageAsync()
    {
        var authenticatedUser = _defaultUser;

        if (await UpdateAndValidateAccessTokenAsync())
        {
            authenticatedUser = CreateAuthenticatedUser(_accessToken!.LoginResponse.AccessToken);
        }

        return new AuthenticationState(authenticatedUser);
    }

    private async Task<bool> UpdateAndValidateAccessTokenAsync()
    {
        try
        {
            var now = DateTime.UtcNow;
            var thirtyMinutesFromNow = now.AddMinutes(TokenExpirationBuffer);

            if (_accessToken is null || thirtyMinutesFromNow > _accessToken.AccessTokenExpiration)
            {
                _accessToken = await _tokenStorageService.GetTokenFromSecureStorageAsync();
            }

            if (_accessToken is null)
            {
                return false;
            }

            // The refresh token expiration is unknown, so we always try to refresh even if the access token expires. It defaults to 14 days.
            // However, we start trying to refresh the access token 30 minutes before it expires to avoid race conditions.
            if (thirtyMinutesFromNow >= _accessToken.AccessTokenExpiration)
            {
                return await RefreshAccessTokenAsync(_accessToken.LoginResponse.RefreshToken);
            }

            return true;
        }
        catch (Exception ex)
        {
            Debug.WriteLine($"Error checking token for validity: {ex}");
            return false;
        }
    }

    private async Task<bool> RefreshAccessTokenAsync(string refreshToken)
    {
        try
        {
            if (refreshToken != null)
            {
                //Call the Refresh endpoint and pass the refresh token
                var response = await _httpClient.PostAsJsonAsync("api/auth/refresh", new { RefreshToken = refreshToken });
                if (response.IsSuccessStatusCode)
                {
                    var result = await response.Content.ReadFromJsonAsync<LoginResponse>();
                    _accessToken = await _tokenStorageService.SaveTokenToSecureStorageAsync(result);
                    return true;
                }

                return false;
            }

            return false;
        }
        catch (Exception ex)
        {
            Debug.WriteLine($"Error refreshing access token: {ex}");
            throw;
        }
    }

    private ClaimsPrincipal CreateAuthenticatedUser(string token)
    {
        var tokenProtector = _dataProtectionProvider.CreateProtector("HybridTodo", BearerTokenDefaults.AuthenticationScheme);
        var ticketFormat = new TicketDataFormat(tokenProtector);

        AuthenticationTicket bearerTicket = ticketFormat.Unprotect(token);

        var principal = bearerTicket.Principal;

        return principal;
    }
}