using CyberFactory.Basics.Constants.Editor;
using CyberFactory.Basics.Extensions;
using CyberFactory.Basics.Objects;
using CyberFactory.Plants.Core.Components.View;
using CyberFactory.Products.Events;
using Cysharp.Threading.Tasks;
using Scellecs.Morpeh;
using Scellecs.Morpeh.Providers;
using Scellecs.Morpeh.Systems;
using UnityEngine;
using UnityEngine.AddressableAssets;
using Timer = CyberFactory.Common.Timer.Timer;

namespace CyberFactory.Products.Ghost {
    [CreateAssetMenu(menuName = AssetMenu.Plants.SYSTEM + "Product Ghost Create", order = AssetMenu.Plants.ORDER_VIEW)]
    public sealed class ProductGhostCreateSystem : Initializer {

        private DisposableTracker disposable;

        public override void OnAwake() {
            disposable = new DisposableTracker();
            World.GetEvent<ProductCreatedEvent>().Subscribe(events => {
                foreach (var e in events) {
                    var plantView = e.plantEntity.GetComponent<PlantView>();
                    var ghostConfig = plantView.productGhostConfig;
                    var spriteReference = e.product.icon;
                    var sourcePosition = plantView.transform.position;

                    CreateGhost(ghostConfig, spriteReference, sourcePosition).Forget();
                }
            }).AddTo(disposable);
        }

        public override void Dispose() {
            disposable.Dispose();
        }

        private async UniTaskVoid CreateGhost(ProductGhostConfig ghostConfig, AssetReferenceSprite spriteReference, Vector3 sourcePosition) {

            // Create GameObject
            var gameObject = new GameObject("ProductGhost", typeof(EntityProvider), typeof(SpriteRenderer));
            var token = gameObject.GetCancellationTokenOnDestroy();
            var sprite = await Addressables.LoadAssetAsync<Sprite>(spriteReference).WithCancellation(token);
            var spriteRenderer = gameObject.GetComponent<SpriteRenderer>();
            spriteRenderer.sprite = sprite;
            spriteRenderer.SetColorAlpha(0); // predefine color value to avoid glitches
            gameObject.transform.position = sourcePosition + ghostConfig.startOffset;
            gameObject.transform.localScale = new Vector3(0.05f, 0.05f, 0.05f); // todo: scale (implement prefab usage)


            // Create Entity
            var entity = gameObject.GetComponent<EntityProvider>().Entity;
            entity.AddComponent<ProductGhost>() = new ProductGhost {
                // todo: async
                spriteRenderer = spriteRenderer,
                config = ghostConfig,
                transform = gameObject.transform,
                sourcePosition = sourcePosition,
            };
            entity.AddComponent<Timer>() = new Timer {
                interval = ghostConfig.duration
            };
        }

    }
}