using CyberFactory.Basics.Constants.Editor;
using CyberFactory.Basics.Extensions;
using CyberFactory.Common.Components;
using CyberFactory.Inventories.Components;
using CyberFactory.Inventories.Queries;
using CyberFactory.Inventories.Services;
using CyberFactory.Products.Components;
using Scellecs.Morpeh;
using Scellecs.Morpeh.Systems;
using UnityEngine;
using VContainer;

namespace CyberFactory.Inventories.Systems {
    /// <summary>
    /// Pull items to the inventory <br/><br/>
    /// note: Order [Last] but before [InventoryServiceSyncSystem]
    /// </summary>
    [CreateAssetMenu(menuName = AssetMenu.Inventory.SYSTEM + "Pull", fileName = nameof(InventoryPullSystem), order = AssetMenu.Inventory.ORDER)]
    public class InventoryPullSystem : UpdateSystem {

        [Inject] private InventoryService Inventory { get; init; }

        private Filter pullItems;

        public override void OnAwake() {
            pullItems = World.Filter.With<InventoryItemPullCall>().Build();
        }

        public override void OnUpdate(float deltaTime) {
            foreach (var pullEntity in pullItems) {
                var pullProduct = pullEntity.GetComponent<Product>();

                bool itemExists = Inventory.Has(pullProduct);
                var pullCount = pullEntity.GetComponent<Count>(out bool stackable);

                if (stackable && pullCount <= 0) {
                    Debug.LogWarning("[Inventory] Pull items 'Count' must be > '0' for stackable item");
                    World.RemoveEntity(pullEntity);
                    continue;
                }

                if (itemExists && stackable) { // Add item count - if product exists and stackable
                    var itemEntity = Inventory.Get(pullProduct);

                    ref var itemCount = ref itemEntity.GetComponent<Count>();
                    itemCount.ChangeSmart(pullCount, itemEntity); // add pull count

                    World.RemoveEntity(pullEntity); // no need to remove pull query ('InventoryItemPullCall')

                } else { // Add new item to inventory - if product is not stackable or not exists in inventory before

                    pullEntity.RemoveComponent<InventoryItemPullCall>();
                    pullEntity.AddComponent<InventoryItem>();
                }

                break; // fix for sync inventory items (process only one item `pull` in frame)
            }
        }

    }
}