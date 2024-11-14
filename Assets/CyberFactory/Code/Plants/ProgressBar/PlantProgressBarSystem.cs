using CyberFactory.Basics.Constants.Editor;
using CyberFactory.Common.Components;
using CyberFactory.Plants.Core.Components.View;
using Scellecs.Morpeh;
using Scellecs.Morpeh.Systems;
using UnityEngine;

namespace CyberFactory.Plants.ProgressBar {
    [CreateAssetMenu(menuName = AssetMenu.Plants.SYSTEM + "Progress Bars", fileName = nameof(PlantProgressBarSystem), order = AssetMenu.Plants.ORDER_VIEW)]
    public sealed class PlantProgressBarSystem : LateUpdateSystem {

        private Filter filter;

        public override void OnAwake() {
            filter = World.Filter.With<PlantView>().With<Progress>().Build();
        }

        public override void OnUpdate(float deltaTime) {
            foreach (var plant in filter) {
                var view = plant.GetComponent<PlantView>();
                view.progressBar.fillAmount = plant.GetComponent<Progress>().value;
            }
        }

    }
}