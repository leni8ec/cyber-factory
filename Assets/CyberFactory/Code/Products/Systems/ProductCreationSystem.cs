using CyberFactory.Basics.Constants.Editor;
using CyberFactory.Common.Components;
using CyberFactory.Inventories.Queries;
using CyberFactory.Plants.Events;
using CyberFactory.Products.Components;
using Scellecs.Morpeh;
using Scellecs.Morpeh.Collections;
using Scellecs.Morpeh.Systems;
using UnityEngine;

namespace CyberFactory.Products.Systems {

    [CreateAssetMenu(menuName = AssetMenu.Products.SYSTEM + nameof(ProductCreationSystem), order = AssetMenu.Products.ORDER)]
    public sealed class ProductCreationSystem : Initializer {
        private Filter filter;

        public override void OnAwake() {
            World.GetEvent<ProductionCompleteEvent>().Subscribe(OnPlantProductionComplete);
        }

        private void OnPlantProductionComplete(FastList<ProductionCompleteEvent> events) {
            foreach (var productionCompleteEvent in events) {
                var productEntity = World.CreateEntity();
                productEntity.AddComponent<Product>().model = productionCompleteEvent.product;
                productEntity.AddComponent<Count>().value = 1;

                productEntity.AddComponent<InventoryItemPullCall>();
            }
        }

    }
}