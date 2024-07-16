using CyberFactory.Inventories.Requests;
using CyberFactory.Plants.Components;
using Scellecs.Morpeh;
using Scellecs.Morpeh.Systems;
using UnityEngine;

namespace CyberFactory.Plants.Systems {
    /// <summary>
    /// Auto resource request when plant is idle
    /// </summary>
    [CreateAssetMenu(menuName = "Systems/Plant Resource Request", fileName = nameof(PlantResourceRequestSystem))]
    public sealed class PlantResourceRequestSystem : UpdateSystem {

        private Filter requestsPlants;

        public override void OnAwake() {
            requestsPlants = World.Filter.With<Plant>().Without<ActiveState>().Without<InventoryProductsRequest>().Build();
        }

        public override void OnUpdate(float deltaTime) {
            foreach (var idlePlant in requestsPlants) {
                var recipe = idlePlant.GetComponent<Plant>().model.product.recipe;

                ref var resourceRequest = ref idlePlant.AddComponent<InventoryProductsRequest>();
                resourceRequest.products = recipe;
            }
        }
    }
}