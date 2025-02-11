using System.Threading;

namespace Ited.HttpFormatter.Internal;

internal static class NoSynchronizationContextScope
{
    public static DisposingAction Enter()
    {
        var context = SynchronizationContext.Current;
        SynchronizationContext.SetSynchronizationContext(null);
        return new DisposingAction(() => SynchronizationContext.SetSynchronizationContext(context));
    }

    internal sealed class DisposingAction : IDisposable
    {
        private readonly Action _action;

        public DisposingAction(Action action)
        {
            _action = action;
        }

        public void Dispose()
        {
            _action();
        }
    }
}