using System;
using CyberFactory.Basics.Objects;

namespace CyberFactory.Basics.Extensions {
    public static class DisposableExtensions {

        public static void AddTo(this IDisposable disposable, DisposableTracker disposableToAdd) {
            disposableToAdd.Add(disposable);
        }

    }
}