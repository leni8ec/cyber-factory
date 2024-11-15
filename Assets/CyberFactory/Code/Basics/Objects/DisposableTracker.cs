﻿using System;
using System.Collections.Generic;
using System.Runtime.CompilerServices;

namespace CyberFactory.Basics.Objects {
    public class DisposableTracker : IDisposable {

        private readonly Stack<IDisposable> disposables = new();

        public void Dispose() {
            IDisposable disposable;
            do {
                lock (disposables) {
                    disposable = disposables.Count > 0
                        ? disposables.Pop()
                        : null;
                }
                disposable?.Dispose();
            } while (disposable != null);
        }

        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public void Add(IDisposable disposable) {
            lock (disposables) {
                disposables.Push(disposable);
            }
        }

    }
}