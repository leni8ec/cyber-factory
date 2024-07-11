using CyberFactory.Common.Components;
using CyberFactory.Inventories.Components;
using CyberFactory.Products.Components;
using Scellecs.Morpeh;
using Scellecs.Morpeh.Systems;
using UnityEngine;

namespace CyberFactory.Inventories.Systems {

    [CreateAssetMenu(fileName = nameof(InventoryPullSystem), menuName = "Systems/Inventory Pull")]
    public class InventoryPullSystem : UpdateSystem {

        private Filter pullItems;
        private Filter inventoryFilter;

        public override void OnAwake() {
            pullItems = World.Filter.With<PullToInventory>().Build();
            inventoryFilter = World.Filter.With<Inventory>().Build();
        }

        public override void OnUpdate(float deltaTime) {
            var service = inventoryFilter.FirstOrDefault().GetComponent<Inventory>().service;

            foreach (var pullItem in pullItems) {
                pullItem.RemoveComponent<PullToInventory>();
                var pullProduct = pullItem.GetComponent<Product>();

                bool stackable = pullItem.Has<Count>();
                bool itemExists = service.Has(pullProduct);
                if (stackable && itemExists) { // Add item count - if product exists and stackable

                    long pullCount = pullItem.GetComponent<Count>().value;
                    if (pullCount <= 0) Debug.LogError("The 'Count' must be > '0'");

                    // add count
                    var inventoryItem = service.Get(pullProduct);
                    inventoryItem.GetComponent<Count>().value += pullCount;

                    inventoryItem.AddComponent<Changes>();
                    World.RemoveEntity(pullItem);

                } else { // Add new entry to inventory - if product is not stackable or not exists in inventory before

                    if (stackable && pullItem.GetComponent<Count>().value <= 0) Debug.LogError("The 'Count' must be > '0'");
                    pullItem.AddComponent<InventoryItem>();
                }

            }
        }

    }

}