using System;
using CyberFactory.Basics;
using CyberFactory.Basics.Constants.Editor;
using CyberFactory.Basics.Extensions;
using CyberFactory.Common.Components;
using CyberFactory.Inventories.Components;
using CyberFactory.Inventories.Events;
using CyberFactory.Inventories.Services;
using CyberFactory.Products.Components;
using CyberFactory.Products.Models;
using Scellecs.Morpeh;
using Scellecs.Morpeh.Systems;
using UnityEngine;

namespace CyberFactory.Inventories.Systems {

    /// <summary>
    /// Create inventory service and sync it with inventory entities ('InventoryItem')
    /// <para/> note: Order - [Last] in inventories
    /// </summary>
    [CreateAssetMenu(menuName = AssetMenu.Inventory.SYSTEM + "Service Sync", fileName = nameof(InventoryServiceSyncSystem), order = AssetMenu.Inventory.ORDER)]
    public class InventoryServiceSyncSystem : UpdateSystem {

        private InventoryService service;

        private Filter itemsChangedCountFilter;
        private SystemStateProcessor<InventorySynchronizedState> inventoryStateProcessor;

        private DisposableTracker disposable;

        public override void OnAwake() {
            Debug.Log("InventoryServiceSyncSystem OnAwake");
            disposable = new DisposableTracker();
            service = new InventoryService();

            ref var inventoryComponent = ref World.CreateEntity().AddComponent<Inventory>();
            inventoryComponent.service = service;

            itemsChangedCountFilter = World.Filter.With<Product>().With<InventoryItem>().With<ChangedCount>().Build();
            inventoryStateProcessor = World.Filter.With<Product>().With<InventoryItem>().ToSystemStateProcessor(AddedToInventory, RemovedFromInventory);

            // Debug log
            World.GetEvent<InventoryItemChangedCountEvent>().Subscribe(events => {
                foreach (var itemChangedEvent in events)
                    Debug.Log($"[Inventory] Count changed Event: {{ {itemChangedEvent.product.name}: '{itemChangedEvent.newCount}' }}");
            }).AddTo(disposable);
        }

        public override void Dispose() {
            disposable.Dispose();
            inventoryStateProcessor.Dispose();
            service.Dispose();
            service = null;
        }

        public override void OnUpdate(float deltaTime) {
            inventoryStateProcessor.Process();

            bool toCommit = false;
            foreach (var entity in itemsChangedCountFilter) {

                var changedCount = entity.GetComponent<ChangedCount>();
                entity.RemoveComponent<ChangedCount>();

                var product = entity.GetComponent<Product>();
                OnItemCountChanged(product, changedCount.oldValue, changedCount.newValue);

                // check to remove item if count is '0'
                if (changedCount.newValue <= 0) {
                    World.RemoveEntity(entity);
                    toCommit = true;

                    if (changedCount.newValue < 0) Debug.LogWarning($"[Inventory] remaining item count must be > 0! (Counts - old: [{changedCount.oldValue}] new [{changedCount.newValue})");
                }
            }
            if (toCommit) World.Commit();
        }


        private InventorySynchronizedState AddedToInventory(Entity entity) {
            var product = entity.GetComponent<Product>();
            bool hasAdded = service.TrySyncOnAdd(product, entity);
            if (!hasAdded) {
                Debug.LogWarning("[Inventory] service sync issue - added item is already exists (it's not stackable)");
                return default;
            }

            int count = product.model.stackable ? service.Count(product.model) : 1;
            OnItemAdded(product.model, count);

            return new InventorySynchronizedState { product = product };
        }

        private void RemovedFromInventory(ref InventorySynchronizedState state) {
            bool hasRemoved = service.TrySyncOnRemove(state.product);
            if (!hasRemoved) Debug.LogWarning($"[Inventory] service sync issue - remove item {state.product.model.name} isn't exists");

            OnItemRemoved(state.product.model);
        }


        private void OnItemAdded(ProductModel product, int count) {
            Debug.Log($"[Inventory] Added: {product.name} (count: {count})");

            World.GetEvent<InventoryItemCreatedEvent>().NextFrame(
                new InventoryItemCreatedEvent { product = product });

            // Inventory refill event
            World.GetEvent<InventoryItemRefillEvent>().NextFrame(
                new InventoryItemRefillEvent { product = product, count = 1 });
        }

        private void OnItemRemoved(ProductModel product) {
            Debug.Log($"[Inventory] Removed: {product.name}");
            World.GetEvent<InventoryItemRemovedEvent>().NextFrame(
                new InventoryItemRemovedEvent {
                    product = product
                });
        }

        private void OnItemCountChanged(Product product, int oldCount, int newCount) {
            Debug.Log($"[Inventory] Count changed ({newCount - oldCount:+#;-#;0}): {{ {product.model.name}: '{newCount}' }}");
            World.GetEvent<InventoryItemChangedCountEvent>().NextFrame(
                new InventoryItemChangedCountEvent {
                    product = product.model,
                    oldCount = oldCount,
                    newCount = newCount
                });

            // Inventory refill event
            if (newCount > oldCount) {
                World.GetEvent<InventoryItemRefillEvent>().NextFrame(
                    new InventoryItemRefillEvent { product = product.model, count = newCount });
            }
        }

    }

    // todo: what is it !?
    [Serializable]
    public struct InventorySynchronizedState : ISystemStateComponent {
        public Product product;
    }

}