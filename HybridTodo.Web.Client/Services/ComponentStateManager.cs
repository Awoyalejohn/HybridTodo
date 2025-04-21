using HybridTodo.Shared.Services;
using Microsoft.AspNetCore.Components;

namespace HybridTodo.Web.Client.Services;

// This class abstracts the use of PersistentComponentState, which is only supported in Blazor Web apps.
// In Blazor Web apps, this implementation delegates to the actual PersistentComponentState.
// In Blazor Hybrid apps (where PersistentComponentState is not available), a no operation implementation is used instead.
// This allows shared components to use component state persistence without platform-specific code.
public class ComponentStateManager(PersistentComponentState persistentComponentState) : IComponentStateManager
{
    public void PersistAsJson<TValue>(string key, TValue instance) => persistentComponentState.PersistAsJson(key, instance);

    public PersistingComponentStateSubscription RegisterOnPersisting(Func<Task> callback) => persistentComponentState.RegisterOnPersisting(callback);

    public bool TryTakeFromJson<TValue>(string key, out TValue? instance) => persistentComponentState.TryTakeFromJson(key, out instance);
}
