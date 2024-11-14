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
    /// Start production when resource request is approved
    /// </summary>
    [CreateAssetMenu(menuName = AssetMenu.Plants.SYSTEM + "Auto Start", fileName = nameof(PlantProductionStartSystem), order = AssetMenu.Plants.ORDER)]
    public sealed class PlantProductionStartSystem : UpdateSystem {

        private Filter readyPlants;

        public override void OnAwake() {
            readyPlants = World.Filter
                .With<Plant>().With<ActiveState>().With<InventoryProductsOrder>().With<OrderApprovedState>()
                .Without<Progress>()
                .Build();
        }

        public override void OnUpdate(float deltaTime) {
            // Start Production
            foreach (var readyPlant in readyPlants) {
                readyPlant.RemoveComponent<InventoryProductsOrder>();
                readyPlant.RemoveComponent<OrderApprovedState>();
                readyPlant.AddComponent<Progress>();
            }
        }
    }
}