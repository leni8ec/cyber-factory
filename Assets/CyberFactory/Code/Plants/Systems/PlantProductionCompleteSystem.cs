using CyberFactory.Common.Components;
using CyberFactory.Inventories.Requests;
using CyberFactory.Plants.Components;
using CyberFactory.Products.Components;
using Scellecs.Morpeh;
using Scellecs.Morpeh.Systems;
using UnityEngine;

namespace CyberFactory.Plants.Systems {
    [CreateAssetMenu(menuName = "Systems/Plant Production Complete", fileName = nameof(PlantProductionCompleteSystem))]
    public class PlantProductionCompleteSystem : UpdateSystem {

        private Filter filter;

        public override void OnAwake() {
            filter = World.Filter.With<Plant>().With<ProductionComplete>().Build();
        }

        public override void OnUpdate(float deltaTime) {
            foreach (var entity in filter) {
                var plant = entity.GetComponent<Plant>().model;

                entity.RemoveComponent<ProductionComplete>();

                var productEntity = World.CreateEntity();
                productEntity.AddComponent<Product>().model = plant.product;
                productEntity.AddComponent<Count>().value = 1;
                productEntity.AddComponent<InventoryItemPullRequest>();
            }
        }

    }
}