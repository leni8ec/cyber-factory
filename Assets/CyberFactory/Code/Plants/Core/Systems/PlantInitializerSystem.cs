using CyberFactory.Basics.Constants.Editor;
using CyberFactory.Common.States;
using CyberFactory.Plants.Core.Components;
using Scellecs.Morpeh;
using Scellecs.Morpeh.Systems;
using UnityEngine;

namespace CyberFactory.Plants.Core.Systems {
    [CreateAssetMenu(menuName = AssetMenu.Plants.SYSTEM + "Initializer", fileName = nameof(PlantInitializerSystem), order = AssetMenu.Plants.ORDER)]
    public class PlantInitializerSystem : UpdateSystem {

        private Filter plantsToInit;

        public override void OnAwake() {
            plantsToInit = World.Filter.With<Plant>().Without<InitializedState>().Build();
        }

        public override void OnUpdate(float deltaTime) {
            foreach (var entity in plantsToInit) {
                entity.GetComponent<Plant>().level = 1;
                entity.AddComponent<InitializedState>();
            }
        }

    }
}