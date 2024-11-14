using CyberFactory.Basics.Constants.Editor;
using CyberFactory.Common.Timer;
using Scellecs.Morpeh;
using Scellecs.Morpeh.Systems;
using UnityEngine;

namespace CyberFactory.Products.Ghost {
    [CreateAssetMenu(menuName = AssetMenu.Products.SYSTEM + "Product Ghost Destroy", order = AssetMenu.Products.ORDER_VIEW)]
    public sealed class ProductGhostDestroySystem : CleanupSystem {
        private Filter ghosts;

        public override void OnAwake() {
            World.GetStash<ProductGhost>().AsDisposable();
            ghosts = World.Filter.With<ProductGhost>().With<TimerComplete>().Build();
        }

        public override void OnUpdate(float deltaTime) {
            foreach (var ghostEntity in ghosts) {
                ghostEntity.Dispose(); // game object is destroyed in 'ProductGhost'
            }
        }

    }
}