using CyberFactory.Common.Components;
using CyberFactory.Inventories.Components;
using CyberFactory.Inventories.Requests;
using CyberFactory.Products.Components;
using Scellecs.Morpeh;
using Scellecs.Morpeh.Systems;
using UnityEngine;

namespace CyberFactory.Inventories.Systems {
    /// <summary>
    /// Pull items to the inventory
    /// note: Order [Last] but before [InventoryServiceSyncSystem]
    /// </summary>
    [CreateAssetMenu(fileName = nameof(InventoryPullSystem), menuName = "Systems/Inventory Pull")]
    public class InventoryPullSystem : UpdateSystem {

        private Filter pullItems;
        private Filter inventories;

        public override void OnAwake() {
            pullItems = World.Filter.With<InventoryItemPullRequest>().Build();
            inventories = World.Filter.With<Inventory>().Build();
        }

        public override void OnUpdate(float deltaTime) {
            var service = inventories.FirstOrDefault().GetComponent<Inventory>().service;

            foreach (var pullItemEntity in pullItems) {
                var pullProduct = pullItemEntity.GetComponent<Product>();

                bool itemExists = service.Has(pullProduct);
                var pullCount = pullItemEntity.GetComponent<Count>(out bool stackable);

                if (stackable && pullCount <= 0) {
                    Debug.LogWarning("[Inventory] Pull items 'Count' must be > '0'");
                    World.RemoveEntity(pullItemEntity);
                    continue;
                }

                if (itemExists && stackable) { // Add item count - if product exists and stackable
                    var inventoryItemEntity = service.Get(pullProduct);
                    ref var count = ref inventoryItemEntity.GetComponent<Count>();

                    count.Change(pullCount, out var changedCount); // add pull count
                    inventoryItemEntity.AddComponent<ChangedCount>() = changedCount;

                    World.RemoveEntity(pullItemEntity); //  no need to remove 'InventoryItemPullRequest'

                } else { // Add new item to inventory - if product is not stackable or not exists in inventory before

                    pullItemEntity.RemoveComponent<InventoryItemPullRequest>();
                    pullItemEntity.AddComponent<InventoryItem>();
                }

            }
        }

    }
}