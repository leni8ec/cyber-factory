using CyberFactory.Basics.Constants.Editor;
using CyberFactory.Common.Components;
using CyberFactory.Inventories.Requests;
using CyberFactory.Plants.Components;
using Scellecs.Morpeh;
using Scellecs.Morpeh.Systems;
using UnityEngine;

namespace CyberFactory.Plants.Systems {
    /// <summary>
    /// Start production when resource request is approved
    /// </summary>
    [CreateAssetMenu(menuName = AssetMenu.Systems.PLANTS + "Auto Start", fileName = nameof(PlantProductionStartSystem), order = AssetMenu.Systems.PLANTS_ORDER)]
    public sealed class PlantProductionStartSystem : UpdateSystem {

        private Filter readyPlants;

        public override void OnAwake() {
            readyPlants = World.Filter
                .With<Plant>().With<ActiveState>().With<InventoryProductsRequest>().With<RequestApprovedState>()
                .Without<Progress>()
                .Build();
        }

        public override void OnUpdate(float deltaTime) {
            // Start Production
            foreach (var readyPlant in readyPlants) {
                readyPlant.RemoveComponent<InventoryProductsRequest>();
                readyPlant.RemoveComponent<RequestApprovedState>();
                readyPlant.AddComponent<Progress>();
            }
        }
    }
}