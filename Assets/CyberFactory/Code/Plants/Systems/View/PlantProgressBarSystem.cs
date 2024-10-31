using CyberFactory.Common.Components;
using CyberFactory.Plants.Components;
using CyberFactory.Plants.Components.View;
using Scellecs.Morpeh;
using Scellecs.Morpeh.Systems;
using UnityEngine;

namespace CyberFactory.Plants.Systems.View {

    [CreateAssetMenu(menuName = "Systems/Plant Progress Bars", fileName = nameof(PlantProgressBarSystem))]
    public sealed class PlantProgressBarSystem : LateUpdateSystem {

        private Filter filter;

        public override void OnAwake() {
            // filter = World.Filter.With<Plant>().With<PlantView>().With<ProductionComplete>().Build();
            filter = World.Filter.With<Plant>().With<PlantView>().With<Progress>().Build();
        }

        public override void OnUpdate(float deltaTime) {
            foreach (var plant in filter) {
                var view = plant.GetComponent<PlantView>();
                view.progressBar.fillAmount = plant.GetComponent<Progress>().value;
            }
        }

    }
}