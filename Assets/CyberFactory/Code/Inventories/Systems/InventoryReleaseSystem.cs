using CyberFactory.Basics.Constants.Editor;
using CyberFactory.Basics.Extensions;
using CyberFactory.Common.Components;
using CyberFactory.Inventories.Queries;
using CyberFactory.Inventories.Services;
using CyberFactory.Products.Components;
using Scellecs.Morpeh;
using Scellecs.Morpeh.Systems;
using UnityEngine;
using VContainer;

namespace CyberFactory.Inventories.Systems {
    /// <summary>
    /// Release items from the inventory <br/><br/>
    /// note: Order - [First] <br/>
    /// note: not used yet (use 'InventoryRequestSystem' instead)
    /// </summary>
    [CreateAssetMenu(menuName = AssetMenu.Inventory.SYSTEM + "Release", fileName = nameof(InventoryReleaseSystem), order = AssetMenu.Inventory.ORDER)]
    public class InventoryReleaseSystem : UpdateSystem {

        [Inject] private InventoryService Inventory { get; init; }

        private Filter releaseItems;

        public override void OnAwake() {
            releaseItems = World.Filter.With<Product>().With<InventoryItemReleaseCall>().Build();
        }

        public override void OnUpdate(float deltaTime) {
            foreach (var releaseItem in releaseItems) {
                releaseItem.RemoveComponent<InventoryItemReleaseCall>();
                var releaseProduct = releaseItem.GetComponent<Product>();

                var itemEntity = Inventory.TryGet(releaseProduct, out bool itemExists);
                if (!itemExists) {
                    Debug.LogWarning("[Inventory] Item to release - isn't exists");
                    World.RemoveEntity(releaseItem);
                    return;
                }

                var releaseCount = releaseItem.GetComponent<Count>(out bool stackable);
                bool removeInventoryItem = !stackable;
                if (stackable) { // Release item count - if product exists and stackable
                    if (releaseCount <= 0) {
                        Debug.LogWarning("[Inventory] Release items 'Count' must be > '0'");
                        World.RemoveEntity(releaseItem);
                        continue;
                    }

                    ref var itemCount = ref itemEntity.GetComponent<Count>();
                    if (releaseCount.value > itemCount.value)
                        Debug.LogWarning($"[Inventory] item count [{itemCount.value}] isn't enough to release [{releaseCount.value}]");

                    itemCount.ChangeSmart(-releaseCount, itemEntity); // subtract release count

                    if (itemCount.value <= 0) removeInventoryItem = true;
                }

                World.RemoveEntity(releaseItem);
                if (removeInventoryItem) World.RemoveEntity(itemEntity); // Remove item from inventory - if item is not stackable or remaining count == 0 
            }
        }

    }
}