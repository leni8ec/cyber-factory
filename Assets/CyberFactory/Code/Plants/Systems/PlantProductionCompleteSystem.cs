using CyberFactory.Common.Components;
using CyberFactory.Inventories.Components;
using CyberFactory.Plants.Components;
using CyberFactory.Products.Components;
using Scellecs.Morpeh;
using Scellecs.Morpeh.Systems;
using UnityEngine;

namespace CyberFactory.Plants.Systems {

    [CreateAssetMenu(fileName = nameof(PlantProductionCompleteSystem), menuName = "Systems/Plant Production Complete")]
    public class PlantProductionCompleteSystem : UpdateSystem {

        private Filter filter;

        public override void OnAwake() {
            filter = World.Filter.With<Plant>().With<ProductionComplete>().Build();
        }

        public override void OnUpdate(float deltaTime) {
            foreach (var entity in filter) {
                var plant = entity.GetComponent<Plant>().plant;

                entity.RemoveComponent<ProductionComplete>();

                var productEntity = World.CreateEntity();
                productEntity.AddComponent<Product>().product = plant.product;
                productEntity.AddComponent<Count>().value = 1;
                productEntity.AddComponent<PullToInventory>();
            }
        }

    }

}