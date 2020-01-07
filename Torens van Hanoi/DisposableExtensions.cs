using System;
using System.Reactive.Disposables;

namespace TowersOfHanoi
{
    public static class DisposableExtensions
    {
        public static IDisposable DisposeWith(this IDisposable disposable, CompositeDisposable disposer)
        {
            if (disposable != null)
            {
                disposer.Add(disposable);
            }

            return disposable;
        }
    }
}