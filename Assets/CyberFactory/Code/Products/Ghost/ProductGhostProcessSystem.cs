using CyberFactory.Basics.Constants.Editor;
using CyberFactory.Basics.Extensions;
using CyberFactory.Common.Timer;
using Scellecs.Morpeh;
using Scellecs.Morpeh.Systems;
using UnityEngine;

namespace CyberFactory.Products.Ghost {
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
                var config = ghost.config;

                float progress = timer.Progress;

                if (progress <= 0.7f) {
                    var targetPos = ghost.sourcePosition + config.startOffset + (config.moveDistance * timer.Progress);
                    ghost.transform.localPosition = targetPos;

                    if (progress < 0.2f) {
                        float alpha = progress * 5;
                        ghost.spriteRenderer.SetColorAlpha(alpha);
                    }
                } else {
                    float alpha = 1 - (progress - 0.7f) / 0.3f;
                    ghost.spriteRenderer.SetColorAlpha(alpha);
                }
            }
        }

    }
}