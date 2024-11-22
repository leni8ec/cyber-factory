using System;
using System.Collections.Concurrent;
using System.Threading;
using Cysharp.Threading.Tasks;
using Scellecs.Morpeh;

namespace CyberFactory.Common.Pool {
    public class EntityPoolAsync {
        // todo: migrate to generic version
        // todo: implement disposable to destroy objects/entities
        // todo: maybe use Dictionary (instead of stack)? (to safe from duplicate and fast get main component or gameobject)
        private readonly ConcurrentStack<Entity> pool = new();

        private readonly Func<CancellationToken, UniTask<Entity>> asyncFactoryMethod;

        public EntityPoolAsync(Func<CancellationToken, UniTask<Entity>> asyncFactoryMethod) {
            this.asyncFactoryMethod = asyncFactoryMethod;
        }

        public async UniTask PrepareAsync(int initSize, CancellationToken token) {
            for (int i = 0; i < initSize; i++) {
                Return(await asyncFactoryMethod(token));
            }
        }

        public async UniTask<Entity> GetAsync(CancellationToken token) {
            if (pool.TryPop(out var entity)) {
                return entity;
            }

            return await asyncFactoryMethod(token);
        }

        public void Return(Entity entity) {
            pool.Push(entity);
        }

    }
}