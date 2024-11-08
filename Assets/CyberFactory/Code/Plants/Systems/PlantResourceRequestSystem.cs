using CyberFactory.Basics.Constants.Editor;
using CyberFactory.Common.Components;
using CyberFactory.Inventories.Requests;
using CyberFactory.Plants.Components;
using Scellecs.Morpeh;
using Scellecs.Morpeh.Systems;
using UnityEngine;

namespace CyberFactory.Plants.Systems {
    /// <summary>
    /// Auto resource request when plant is idle
    /// </summary>
    [CreateAssetMenu(menuName = AssetMenu.Systems.PLANTS + "Resource Request", fileName = nameof(PlantResourceRequestSystem), order = AssetMenu.Systems.PLANTS_ORDER)]
    public sealed class PlantResourceRequestSystem : UpdateSystem {

        private Filter requestsPlants;

        public override void OnAwake() {
            requestsPlants = World.Filter
                .With<Plant>().With<ActiveState>()
                .Without<Progress>().Without<InventoryProductsRequest>()
                .Build();
        }

        public override void OnUpdate(float deltaTime) {
            foreach (var idlePlant in requestsPlants) {
                var plant = idlePlant.GetComponent<Plant>().model;
                if (!plant) {
                    Debug.LogError($"Plant model is null");
                    return;
                }

                var recipe = plant.product.recipe;
                ref var resourceRequest = ref idlePlant.AddComponent<InventoryProductsRequest>();
                resourceRequest.products = recipe;
            }
        }
    }
}