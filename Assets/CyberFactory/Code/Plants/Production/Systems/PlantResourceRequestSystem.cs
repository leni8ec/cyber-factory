using CyberFactory.Basics.Constants.Editor;
using CyberFactory.Common.Components;
using CyberFactory.Common.States;
using CyberFactory.Inventories.Queries;
using CyberFactory.Plants.Core.Components;
using Scellecs.Morpeh;
using Scellecs.Morpeh.Systems;
using UnityEngine;

namespace CyberFactory.Plants.Production.Systems {
    /// <summary>
    /// Auto resource request when plant is active but idle
    /// </summary>
    [CreateAssetMenu(menuName = AssetMenu.Plants.SYSTEM + "Resource Request", fileName = nameof(PlantResourceRequestSystem), order = AssetMenu.Plants.ORDER)]
    public sealed class PlantResourceRequestSystem : UpdateSystem {

        private Filter requestsPlants;

        public override void OnAwake() {
            requestsPlants = World.Filter
                .With<Plant>().With<ActiveState>()
                .Without<Progress>().Without<InventoryProductsOrder>()
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
                ref var resourceRequest = ref idlePlant.AddComponent<InventoryProductsOrder>();
                resourceRequest.products = recipe;
            }
        }
    }
}