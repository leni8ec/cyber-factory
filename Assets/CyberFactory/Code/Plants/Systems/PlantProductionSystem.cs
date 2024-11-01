using CyberFactory.Basics.Constants.Editor;
using CyberFactory.Common.Components;
using CyberFactory.Plants.Components;
using Scellecs.Morpeh;
using Scellecs.Morpeh.Systems;
using UnityEngine;

namespace CyberFactory.Plants.Systems {
    [CreateAssetMenu(menuName = AssetMenu.Systems.PLANTS + "Production", fileName = nameof(PlantProductionSystem), order = AssetMenu.Systems.PLANTS_ORDER)]
    public class PlantProductionSystem : UpdateSystem {

        private Filter filter;

        public override void OnAwake() {
            filter = World.Filter.With<Plant>().With<ActiveState>().With<Progress>().Build();
        }

        public override void OnUpdate(float deltaTime) {
            foreach (var entity in filter) {
                ref var progress = ref entity.GetComponent<Progress>();
                var plant = entity.GetComponent<Plant>();

                float delta = deltaTime * plant.ProductionRate;
                progress.value += delta;

                if (progress.IsComplete) {
                    entity.RemoveComponent<Progress>();
                    entity.AddComponent<ProductionComplete>();
                }

            }
        }

    }
}