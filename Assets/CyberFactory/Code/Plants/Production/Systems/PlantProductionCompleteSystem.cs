using CyberFactory.Basics.Constants.Editor;
using CyberFactory.Plants.Core.Components;
using CyberFactory.Plants.Events;
using Scellecs.Morpeh;
using Scellecs.Morpeh.Systems;
using UnityEngine;

namespace CyberFactory.Plants.Production.Systems {
    [CreateAssetMenu(menuName = AssetMenu.Plants.SYSTEM + "Production Complete", fileName = nameof(PlantProductionCompleteSystem), order = AssetMenu.Plants.ORDER)]
    public class PlantProductionCompleteSystem : UpdateSystem {

        private Filter filter;

        public override void OnAwake() {
            filter = World.Filter.With<Plant>().With<ProductionComplete>().Build();
        }

        public override void OnUpdate(float deltaTime) {
            foreach (var entity in filter) {
                var plant = entity.GetComponent<Plant>().model;

                // todo: stopped this
                World.GetEvent<ProductionCompleteEvent>().NextFrame(new ProductionCompleteEvent(entity, plant.product));

                entity.RemoveComponent<ProductionComplete>();
            }
        }

    }
}