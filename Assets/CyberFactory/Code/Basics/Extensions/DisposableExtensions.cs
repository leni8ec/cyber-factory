using System;
using CyberFactory.Basics.Objects;

namespace CyberFactory.Basics.Extensions {
    public static class DisposableExtensions {

        public static T AddTo<T>(this T disposable, DisposableTracker disposableToAdd) where T : IDisposable {
            disposableToAdd.Add(disposable);
            return disposable;
        }

    }
}