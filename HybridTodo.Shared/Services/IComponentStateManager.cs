using Microsoft.AspNetCore.Components;

namespace HybridTodo.Shared.Services;

public interface IComponentStateManager
{
    bool TryTakeFromJson<TValue>(string key, out TValue? instance);

    void PersistAsJson<TValue>(string key, TValue instance);

    PersistingComponentStateSubscription RegisterOnPersisting(Func<Task> callback);
}
