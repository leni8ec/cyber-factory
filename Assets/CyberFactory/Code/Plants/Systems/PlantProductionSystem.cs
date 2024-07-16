using CyberFactory.Common.Components;
using CyberFactory.Plants.Components;
using Scellecs.Morpeh;
using Scellecs.Morpeh.Systems;
using UnityEngine;

namespace CyberFactory.Plants.Systems {
    [CreateAssetMenu(menuName = "Systems/Plant Production", fileName = nameof(PlantProductionSystem))]
    public class PlantProductionSystem : UpdateSystem {

        private Filter filter;

        public override void OnAwake() {
            filter = World.Filter.With<Plant>().With<Progress>().With<ActiveState>().Build();
        }

        public override void OnUpdate(float deltaTime) {
            foreach (var entity in filter) {
                ref var progress = ref entity.GetComponent<Progress>();
                var plant = entity.GetComponent<Plant>().model;

                float delta = deltaTime * plant.productionRateLevels[0];
                progress.value += delta;

                if (progress.IsComplete) {
                    entity.RemoveComponent<Progress>();
                    entity.RemoveComponent<ActiveState>();
                    entity.AddComponent<ProductionComplete>();
                }

            }
        }

    }
}