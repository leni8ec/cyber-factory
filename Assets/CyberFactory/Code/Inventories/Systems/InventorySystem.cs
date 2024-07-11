using System;
using CyberFactory.Common.Components;
using CyberFactory.Inventories.Components;
using CyberFactory.Inventories.Events;
using CyberFactory.Inventories.Services;
using CyberFactory.Products.Components;
using Scellecs.Morpeh;
using Scellecs.Morpeh.Systems;
using UnityEngine;

namespace CyberFactory.Inventories.Systems {

    [CreateAssetMenu(fileName = nameof(InventorySystem), menuName = "Systems/Inventory")]
    public class InventorySystem : UpdateSystem {

        private InventoryService service;

        private Filter changedItemsFilter;
        private SystemStateProcessor<InventorySynchronizedState> inventoryStateProcessor;

        public override void OnAwake() {
            service = new InventoryService();
            ref var inventoryComponent = ref World.CreateEntity().AddComponent<Inventory>();
            inventoryComponent.service = service;

            changedItemsFilter = World.Filter.With<Product>().With<InventoryItem>().With<Changes>().Build();
            inventoryStateProcessor = World.Filter.With<Product>().With<InventoryItem>().ToSystemStateProcessor(AddedToInventory, RemovedFromInventory);

            // Debug log
            World.GetEvent<InventoryItemChangedEvent>().Subscribe(events => {
                foreach (var itemChangedEvent in events)
                    Debug.Log($"[Inventory] Changed Event: {itemChangedEvent.product.name} ({itemChangedEvent.count})");
            });
        }

        public override void OnUpdate(float deltaTime) {
            inventoryStateProcessor.Process();
            foreach (var entity in changedItemsFilter) {
                entity.RemoveComponent<Changes>();
                var product = entity.GetComponent<Product>();
                long count = entity.GetComponent<Count>().value;

                Debug.Log($"[Inventory] Changed: {product.product.name} ({count})");
                ItemChanged(product, count);
            }
        }


        private InventorySynchronizedState AddedToInventory(Entity entity) {
            var product = entity.GetComponent<Product>();
            service.Add(product, entity);

            long count = entity.Has<Count>() ? entity.GetComponent<Count>().value : 1;
            ItemChanged(product, count);

            Debug.Log($"[Inventory] Add: {product.product.name} ({count})");
            return new InventorySynchronizedState { product = product };
        }

        private void RemovedFromInventory(ref InventorySynchronizedState state) {
            service.Remove(state.product);
            ItemChanged(state.product, 0);

            Debug.Log($"[Inventory] Remove: {state.product.product.name}");
        }


        private void ItemChanged(Product product, long count) {
            World.GetEvent<InventoryItemChangedEvent>()
                .NextFrame(new InventoryItemChangedEvent { product = product.product, count = count });
        }

    }

    [Serializable]
    public struct InventorySynchronizedState : ISystemStateComponent {
        public Product product;
    }

}