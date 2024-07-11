using CyberFactory.Common;
using Scellecs.Morpeh;
using Scellecs.Morpeh.Systems;
using UnityEngine;

namespace CyberFactory.Plants {

    [CreateAssetMenu(fileName = nameof(PlantProductionSystem), menuName = "Systems/Plant Production")]
    public class PlantProductionSystem : UpdateSystem {

        private Filter filter;

        public override void OnAwake() {
            filter = World.Filter.With<Plant>().With<Progress>().With<ActiveState>().Build();
        }

        public override void OnUpdate(float deltaTime) {
            foreach (var entity in filter) {
                ref var progress = ref entity.GetComponent<Progress>();
                var plant = entity.GetComponent<Plant>().plant;

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