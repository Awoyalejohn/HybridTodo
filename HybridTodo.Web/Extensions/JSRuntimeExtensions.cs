using Microsoft.JSInterop;

namespace HybridTodo.Web.Extensions;

public static class JSRuntimeExtensions
{
    public static async Task<TResult?> TryInvokeAsync<TResult>(this IJSRuntime jsRuntime, string name, params object[] args)
    {
        try
        {
            return await jsRuntime.InvokeAsync<TResult>(name, args);
        }
        catch (JSException)
        {
            return default;
        }
    }
}
