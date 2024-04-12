using System.Diagnostics.CodeAnalysis;
using LinFx.Utils;

namespace LinFx.Extensions.EventBus;

[AttributeUsage(AttributeTargets.Class)]
public class EventNameAttribute([NotNull] string name) : Attribute, IEventNameProvider
{
    public virtual string Name { get; } = Check.NotNullOrWhiteSpace(name, nameof(name));

    public static string GetNameOrDefault<TEvent>() => GetNameOrDefault(typeof(TEvent));

    public static string GetNameOrDefault([NotNull] Type eventType) => eventType
                   .GetCustomAttributes(true)
                   .OfType<IEventNameProvider>()
                   .FirstOrDefault()
                   ?.GetName(eventType)
               ?? eventType.FullName;

    public string GetName(Type eventType) => Name;
}
