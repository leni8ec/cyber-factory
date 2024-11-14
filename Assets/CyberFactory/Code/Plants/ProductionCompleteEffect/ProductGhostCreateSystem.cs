using CyberFactory.Basics.Constants.Editor;
using CyberFactory.Common.Timer;
using CyberFactory.Plants.Core.Components;
using CyberFactory.Plants.Core.Components.View;
using Scellecs.Morpeh;
using Scellecs.Morpeh.Systems;
using UnityEngine;

namespace CyberFactory.Plants.ProductionCompleteEffect {
    [CreateAssetMenu(menuName = AssetMenu.Plants.SYSTEM + "Product Ghost Create", order = AssetMenu.Plants.ORDER_VIEW)]
    public sealed class ProductGhostCreateSystem : LateUpdateSystem {
        private Filter completePlants;

        public override void OnAwake() {
            completePlants = World.Filter.With<Plant>().With<PlantView>().With<ProductionComplete>().Build();
        }

        public override void OnUpdate(float deltaTime) {
            foreach (var plantEntity in completePlants) {
                var plantView = plantEntity.GetComponent<PlantView>();

                plantEntity.RemoveComponent<ProductionComplete>();

                var sprite = (Sprite) plantEntity.GetComponent<Plant>().model.product.icon.Asset;

                // todo: polish this
                var gameObject = new GameObject();
                var spriteRenderer = gameObject.AddComponent<SpriteRenderer>();
                spriteRenderer.sprite = sprite;

                var effectEntity = World.CreateEntity();
                effectEntity.AddComponent<ProductGhost>() = new ProductGhost {
                    // todo: async
                    sprite = sprite,
                    config = plantView.productGhostConfig,
                    transform = gameObject.transform
                };
                effectEntity.AddComponent<Timer>() = new Timer {
                    duration = plantView.productGhostConfig.duration
                };
            }
        }

    }
}