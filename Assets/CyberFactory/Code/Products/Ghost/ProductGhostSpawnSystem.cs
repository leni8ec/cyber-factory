using System.Threading;
using CyberFactory.Basics.Constants.Editor;
using CyberFactory.Basics.Extensions;
using CyberFactory.Basics.Objects;
using CyberFactory.Plants.Core.Components.View;
using CyberFactory.Products.Events;
using Cysharp.Threading.Tasks;
using Scellecs.Morpeh;
using Scellecs.Morpeh.Systems;
using UnityEngine;
using VContainer;

namespace CyberFactory.Products.Ghost {
    [CreateAssetMenu(menuName = AssetMenu.Plants.SYSTEM + "Product Ghost Create", order = AssetMenu.Plants.ORDER_VIEW)]
    public sealed class ProductGhostSpawnSystem : Initializer {

        [Inject] private ProductGhostSpawner spawner;

        private DisposableTracker disposable;
        private CancellationTokenSource tokenSource;

        private const int POOL_INITIAL_SIZE = 3;

        public override void OnAwake() {
            disposable = new DisposableTracker();
            tokenSource = new CancellationTokenSource().AddTo(disposable);

            spawner.PrepareAsync(POOL_INITIAL_SIZE, tokenSource.Token).Forget();

            World.GetEvent<ProductCreatedEvent>().Subscribe(events => {
                foreach (var @event in events) {
                    var plantView = @event.plantEntity.GetComponent<PlantView>();
                    var ghostConfig = plantView.productGhostConfig;
                    var sourcePosition = plantView.transform.position;
                    var spriteReference = @event.product.icon;

                    spawner.SpawnAsync(ghostConfig, spriteReference, sourcePosition, tokenSource.Token).Forget();

                }
            }).AddTo(disposable);
        }

        public override void Dispose() {
            tokenSource.Cancel();
            disposable.Dispose();
        }

    }
}