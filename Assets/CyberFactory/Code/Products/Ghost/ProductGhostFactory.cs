using System.Threading;
using CyberFactory.Basics.Extensions;
using CyberFactory.Common.Services.GameObjects;
using Cysharp.Threading.Tasks;
using Scellecs.Morpeh;
using Scellecs.Morpeh.Providers;
using UnityEngine;
using UnityEngine.AddressableAssets;
using VContainer;
using Timer = CyberFactory.Common.Timer.Timer;

namespace CyberFactory.Products.Ghost {
    public class ProductGhostFactory {

        [Inject] private GameObjectsService GameObjectsService { get; init; }

        public async UniTaskVoid CreateAsync(ProductGhostConfig config, AssetReferenceSprite spriteReference, Vector3 sourcePosition, CancellationToken token) {
            // Create GameObject
            var gameObject = GameObjectsService.Create("Ghosts", "ProductGhost",
                typeof(EntityProvider), typeof(SpriteRenderer));

            // todo: maybe use it later
            // var gameObject = Object.InstantiateAsync(gameObject, 1, parent, sourcePosition, Quaternion.identity, token);

            // Configure components
            var sprite = await Addressables.LoadAssetAsync<Sprite>(spriteReference).WithCancellation(token);
            var spriteRenderer = gameObject.GetComponent<SpriteRenderer>();
            spriteRenderer.sprite = sprite;
            spriteRenderer.SetColorAlpha(0); // predefine color value to avoid glitches
            gameObject.transform.position = sourcePosition + config.startOffset;
            gameObject.transform.localScale = new Vector3(0.05f, 0.05f, 0.05f); // todo: scale (implement prefab usage)


            // Create Entity
            var entity = gameObject.GetComponent<EntityProvider>().Entity;
            entity.AddComponent<ProductGhost>() = new ProductGhost {
                // todo: async
                spriteRenderer = spriteRenderer,
                config = config,
                transform = gameObject.transform,
                sourcePosition = sourcePosition,
            };
            entity.AddComponent<Timer>() = new Timer() {
                interval = config.duration
            };
        }

    }
}