namespace HttpMessageFormatter.Internal;

internal static class NoSynchronizationContextScope
{
    public static void ExecuteInDefaultSynchronizationContext(this Func<Task> asyncFunc)
    {
        var previousContext = SynchronizationContext.Current;
        try
        {
            SynchronizationContext.SetSynchronizationContext(null);
            asyncFunc().GetAwaiter().GetResult();
        }
        finally
        {
            SynchronizationContext.SetSynchronizationContext(previousContext);
        }
    }
}
