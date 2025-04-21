using HybridTodo.Shared.Services;
using Microsoft.AspNetCore.Components;

namespace HybridTodo.Services;

// This class abstracts the use of PersistentComponentState, which is only supported in Blazor Web apps.
// In Blazor Web apps, this implementation delegates to the actual PersistentComponentState.
// In Blazor Hybrid apps (where PersistentComponentState is not available), a no operation implementation is used instead.
// This allows shared components to use component state persistence without platform-specific code.
internal sealed class MauiComponentStateManager : IComponentStateManager
{
    public void PersistAsJson<TValue>(string key, TValue instance)
    {
        return;
    }

    public PersistingComponentStateSubscription RegisterOnPersisting(Func<Task> callback) => default;

    public bool TryTakeFromJson<TValue>(string key, out TValue? instance)
    {
        instance = default;
        return false;
    }
}
