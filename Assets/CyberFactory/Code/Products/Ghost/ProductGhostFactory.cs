using System.Threading;
using CyberFactory.Common.Services.GameObjects;
using Cysharp.Threading.Tasks;
using Scellecs.Morpeh;
using Scellecs.Morpeh.Providers;
using UnityEngine;
using VContainer;

namespace CyberFactory.Products.Ghost {
    public class ProductGhostFactory {

        [Inject] private GameObjectsService GameObjectsService { get; init; }

        // public async UniTaskVoid CreateAsync(ProductGhostConfig config, AssetReferenceSprite spriteReference, Vector3 sourcePosition, CancellationToken token) {
        public async UniTask<Entity> CreateAsync(CancellationToken token) {
            // Create GameObject
            var gameObject = GameObjectsService.Create("Ghosts", "ProductGhost",
                typeof(EntityProvider), typeof(SpriteRenderer));

            // note: maybe use it later
            // var gameObject = Object.InstantiateAsync(gameObject, 1, parent, sourcePosition, Quaternion.identity, token);

            gameObject.SetActive(false);

            // Create Entity
            var entity = gameObject.GetComponent<EntityProvider>().Entity;
            entity.AddComponent<ProductGhost>() = new ProductGhost {
                spriteRenderer = gameObject.GetComponent<SpriteRenderer>(),
                transform = gameObject.transform,
            };

            return entity;
        }

    }
}