using CyberFactory.Common.Components;
using CyberFactory.Plants.Components;
using Scellecs.Morpeh;
using Scellecs.Morpeh.Systems;
using UnityEngine;

namespace CyberFactory.Plants.Systems {
    [CreateAssetMenu(menuName = "Systems/Plant Initializer", fileName = nameof(PlantInitializerSystem))]
    public class PlantInitializerSystem : UpdateSystem {

        private Filter plantsToInit;

        public override void OnAwake() {
            plantsToInit = World.Filter.With<Plant>().Without<Initialized>().Build();
        }

        public override void OnUpdate(float deltaTime) {
            foreach (var entity in plantsToInit) {
                entity.GetComponent<Plant>().level = 1;
                entity.AddComponent<Initialized>();
            }
        }

    }
}