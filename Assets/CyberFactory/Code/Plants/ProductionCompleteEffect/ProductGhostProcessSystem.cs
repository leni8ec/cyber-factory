using CyberFactory.Basics.Constants.Editor;
using CyberFactory.Common.Timer;
using Scellecs.Morpeh;
using Scellecs.Morpeh.Systems;
using UnityEngine;

namespace CyberFactory.Plants.ProductionCompleteEffect {
    [CreateAssetMenu(menuName = AssetMenu.Plants.SYSTEM + "Product Ghost Process", order = AssetMenu.Plants.ORDER_VIEW)]
    public sealed class ProductGhostProcessSystem : LateUpdateSystem {
        private Filter ghosts;

        public override void OnAwake() {
            ghosts = World.Filter.With<ProductGhost>().With<Timer>().Build();
        }

        public override void OnUpdate(float deltaTime) {
            foreach (var ghostEntity in ghosts) {
                var timer = ghostEntity.GetComponent<Timer>();
                var ghost = ghostEntity.GetComponent<ProductGhost>();

                var targetPos = ghost.config.startOffset + ghost.config.moveDistance * timer.Progress;
                ghost.transform.localPosition = targetPos;
            }
        }

    }
}