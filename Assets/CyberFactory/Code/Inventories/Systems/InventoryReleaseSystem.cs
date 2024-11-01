﻿using CyberFactory.Basics.Constants.Editor;
using CyberFactory.Common.Components;
using CyberFactory.Inventories.Components;
using CyberFactory.Inventories.Requests;
using CyberFactory.Products.Components;
using Scellecs.Morpeh;
using Scellecs.Morpeh.Systems;
using UnityEngine;

namespace CyberFactory.Inventories.Systems {
    /// <summary>
    /// Release items from the inventory
    /// note: Order - [First]
    /// note: not used yet (use 'InventoryRequestSystem' instead)
    /// </summary>
    [CreateAssetMenu(menuName = AssetMenu.Systems.INVENTORY + "Release", fileName = nameof(InventoryReleaseSystem), order = AssetMenu.Systems.INVENTORY_ORDER)]
    public class InventoryReleaseSystem : UpdateSystem {

        private Filter releaseItems;
        private Filter inventories;

        public override void OnAwake() {
            releaseItems = World.Filter.With<Product>().With<InventoryItemReleaseRequest>().Build();
            inventories = World.Filter.With<Inventory>().Build();
        }

        public override void OnUpdate(float deltaTime) {
            var service = inventories.FirstOrDefault().GetComponent<Inventory>().service;

            foreach (var releaseItem in releaseItems) {
                releaseItem.RemoveComponent<InventoryItemReleaseRequest>();
                var releaseProduct = releaseItem.GetComponent<Product>();

                var inventoryItem = service.TryGet(releaseProduct, out bool itemExists);
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

                    ref var inventoryCount = ref inventoryItem.GetComponent<Count>();
                    if (releaseCount.value > inventoryCount.value)
                        Debug.LogWarning($"[Inventory] item count [{inventoryCount.value}] isn't enough to release [{releaseCount.value}]");

                    inventoryCount.Change(-releaseCount, out var changedCount); // subtract release count
                    inventoryItem.AddComponent<ChangedCount>() = changedCount;

                    if (inventoryCount.value <= 0) removeInventoryItem = true;
                }

                World.RemoveEntity(releaseItem);
                if (removeInventoryItem) World.RemoveEntity(inventoryItem); // Remove item from inventory - if item is not stackable or remaining count == 0 
            }
        }

    }
}