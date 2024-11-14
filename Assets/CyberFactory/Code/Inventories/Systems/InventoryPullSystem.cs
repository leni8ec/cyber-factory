using CyberFactory.Basics.Constants.Editor;
using CyberFactory.Basics.Extensions;
using CyberFactory.Common.Components;
using CyberFactory.Inventories.Components;
using CyberFactory.Inventories.Queries;
using CyberFactory.Products.Components;
using Scellecs.Morpeh;
using Scellecs.Morpeh.Systems;
using UnityEngine;

namespace CyberFactory.Inventories.Systems {
    /// <summary>
    /// Pull items to the inventory <br/><br/>
    /// note: Order [Last] but before [InventoryServiceSyncSystem]
    /// </summary>
    [CreateAssetMenu(menuName = AssetMenu.Inventory.SYSTEM + "Pull", fileName = nameof(InventoryPullSystem), order = AssetMenu.Inventory.ORDER)]
    public class InventoryPullSystem : UpdateSystem {

        private Filter pullItems;
        private Filter inventories;

        public override void OnAwake() {
            pullItems = World.Filter.With<InventoryItemPullCall>().Build();
            inventories = World.Filter.With<Inventory>().Build();
        }

        public override void OnUpdate(float deltaTime) {
            var service = inventories.FirstOrDefault().GetComponent<Inventory>().service;

            foreach (var pullEntity in pullItems) {
                var pullProduct = pullEntity.GetComponent<Product>();

                bool itemExists = service.Has(pullProduct);
                var pullCount = pullEntity.GetComponent<Count>(out bool stackable);

                if (stackable && pullCount <= 0) {
                    Debug.LogWarning("[Inventory] Pull items 'Count' must be > '0'");
                    World.RemoveEntity(pullEntity);
                    continue;
                }

                if (itemExists && stackable) { // Add item count - if product exists and stackable
                    var itemEntity = service.Get(pullProduct);

                    ref var itemCount = ref itemEntity.GetComponent<Count>();
                    itemCount.ChangeSmart(pullCount, itemEntity); // add pull count

                    World.RemoveEntity(pullEntity); // no need to remove pull query ('InventoryItemPullCall')

                } else { // Add new item to inventory - if product is not stackable or not exists in inventory before

                    pullEntity.RemoveComponent<InventoryItemPullCall>();
                    pullEntity.AddComponent<InventoryItem>();
                }

            }
        }

    }
}