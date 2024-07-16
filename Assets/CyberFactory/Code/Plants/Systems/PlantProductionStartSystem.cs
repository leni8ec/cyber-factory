using CyberFactory.Common.Components;
using CyberFactory.Plants.Components;
using Scellecs.Morpeh;
using Scellecs.Morpeh.Systems;
using UnityEngine;

namespace CyberFactory.Plants.Systems {
    /// <summary>
    /// Start production when resource request is approved
    /// </summary>
    [CreateAssetMenu(menuName = "Systems/Plant Auto Start", fileName = nameof(PlantProductionStartSystem))]
    public sealed class PlantProductionStartSystem : UpdateSystem {

        private Filter readyPlants;

        public override void OnAwake() {
            readyPlants = World.Filter.With<Plant>().Without<Progress>().Without<ActiveState>().With<RequestApprovedState>().Build();
        }

        public override void OnUpdate(float deltaTime) {
            // Start Production
            foreach (var readyPlant in readyPlants) {
                readyPlant.RemoveComponent<RequestApprovedState>();
                readyPlant.AddComponent<ActiveState>();
                readyPlant.AddComponent<Progress>();
            }
        }
    }
}