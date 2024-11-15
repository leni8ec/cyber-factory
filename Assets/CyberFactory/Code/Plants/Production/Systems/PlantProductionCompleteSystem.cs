using CyberFactory.Basics.Constants.Editor;
using CyberFactory.Plants.Core.Components;
using CyberFactory.Products.Requests;
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

                int count = 1; // default production count value (move it to config later)
                World.GetRequest<ProductCreateRequest>().Publish(new ProductCreateRequest(entity, plant.product, count));

                entity.RemoveComponent<ProductionComplete>();
            }
        }

    }
}