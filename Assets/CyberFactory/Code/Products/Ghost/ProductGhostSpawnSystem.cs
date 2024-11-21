using System.Threading;
using CyberFactory.Basics.Constants.Editor;
using CyberFactory.Basics.Extensions;
using CyberFactory.Basics.Objects;
using CyberFactory.Plants.Core.Components.View;
using CyberFactory.Products.Events;
using Scellecs.Morpeh;
using Scellecs.Morpeh.Systems;
using UnityEngine;
using VContainer;

namespace CyberFactory.Products.Ghost {
    [CreateAssetMenu(menuName = AssetMenu.Plants.SYSTEM + "Product Ghost Create", order = AssetMenu.Plants.ORDER_VIEW)]
    public sealed class ProductGhostSpawnSystem : Initializer {

        [Inject] private ProductGhostFactory Factory { get; init; }

        private DisposableTracker disposable;
        private CancellationTokenSource tokenSource;

        public override void OnAwake() {
            disposable = new DisposableTracker();
            tokenSource = new CancellationTokenSource().AddTo(disposable);

            World.GetEvent<ProductCreatedEvent>().Subscribe(events => {
                foreach (var e in events) {
                    var plantView = e.plantEntity.GetComponent<PlantView>();
                    var sourcePosition = plantView.transform.position;
                    var ghostConfig = plantView.productGhostConfig;
                    var spriteReference = e.product.icon;

                    Factory.CreateAsync(ghostConfig, spriteReference, sourcePosition, tokenSource.Token).Forget();
                }
            }).AddTo(disposable);
        }

        public override void Dispose() {
            tokenSource.Cancel();
            disposable.Dispose();
        }

    }
}