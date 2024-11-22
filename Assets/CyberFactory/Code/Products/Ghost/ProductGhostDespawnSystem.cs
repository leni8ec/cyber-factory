using CyberFactory.Basics.Constants.Editor;
using CyberFactory.Common.Timer;
using Scellecs.Morpeh;
using Scellecs.Morpeh.Systems;
using UnityEngine;
using VContainer;

namespace CyberFactory.Products.Ghost {
    [CreateAssetMenu(menuName = AssetMenu.Products.SYSTEM + "Product Ghost Destroy", order = AssetMenu.Products.ORDER_VIEW)]
    public sealed class ProductGhostDespawnSystem : CleanupSystem {

        [Inject] private ProductGhostSpawner Spawner { get; init; }

        private Filter ghosts;

        public override void OnAwake() {
            World.GetStash<ProductGhost>().AsDisposable();
            ghosts = World.Filter.With<ProductGhost>().With<TimerComplete>().Build();
        }

        public override void OnUpdate(float deltaTime) {
            foreach (var ghostEntity in ghosts) Despawn(ghostEntity);
        }

        private void Despawn(Entity entity) {
            Spawner.Despawn(entity);
        }

    }
}