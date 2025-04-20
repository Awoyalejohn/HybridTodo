using HybridTodo.Abstractions.Services;
using HybridTodo.Shared.Constants;
using HybridTodo.Shared.DTOs;
using System.Diagnostics;
using System.Text.Json;

namespace HybridTodo.Services;

internal sealed class TokenStorageService : ITokenStorageService
{
    public void RemoveToken()
    {
        SecureStorage.Remove(AuthConstants.AccessToken);
    }

    public async Task<AccessTokenInfo?> GetTokenFromSecureStorageAsync()
    {
        try
        {
            var token = await SecureStorage.GetAsync(AuthConstants.AccessToken);

            if (!string.IsNullOrEmpty(token))
            {
                return JsonSerializer.Deserialize<AccessTokenInfo>(token);
            }
        }
        catch (Exception ex)
        {
            Debug.WriteLine("Unable to retrieve AccessTokenInfo from SecureStorage." + ex);
        }
        return null;
    }

    public async Task<AccessTokenInfo?> SaveTokenToSecureStorageAsync(LoginResponse loginModel)
    {
        AccessTokenInfo? accessToken = null;
        try
        {
            if (loginModel != null)
            {
                DateTime tokenExpiration = DateTime.UtcNow.AddSeconds(loginModel.ExpiresIn);

                accessToken = new AccessTokenInfo
                {
                    LoginResponse = loginModel,
                    //Email = email,
                    AccessTokenExpiration = tokenExpiration
                };

                await SecureStorage.SetAsync(AuthConstants.AccessToken, JsonSerializer.Serialize<AccessTokenInfo>(accessToken));
            }
        }
        catch (Exception ex)
        {
            Debug.WriteLine("Unable to save AccessTokenInfo to SecureStorage." + ex);
            accessToken = null;
        }
        return accessToken;
    }
}
