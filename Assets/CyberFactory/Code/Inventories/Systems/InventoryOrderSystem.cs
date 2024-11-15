using System;
using CyberFactory.Basics;
using CyberFactory.Basics.Constants.Editor;
using CyberFactory.Basics.Extensions;
using CyberFactory.Common.Components;
using CyberFactory.Common.States;
using CyberFactory.Inventories.Components;
using CyberFactory.Inventories.Events;
using CyberFactory.Inventories.Queries;
using CyberFactory.Inventories.Services;
using Scellecs.Morpeh;
using Scellecs.Morpeh.Collections;
using Scellecs.Morpeh.Systems;
using UnityEngine;

namespace CyberFactory.Inventories.Systems {

    /// <summary>
    /// Process inventory orders and release items to their clients (ex: plants) <br/><br/>
    /// note: Order - [Second] (or [First] if without 'Release' system)
    /// </summary>
    [CreateAssetMenu(menuName = AssetMenu.Inventory.SYSTEM + "Order", fileName = nameof(InventoryOrderSystem), order = AssetMenu.Inventory.ORDER)]
    public class InventoryOrderSystem : UpdateSystem {

        private Filter inventoryFilter;
        private InventoryService inventory;
        /// New incoming orders
        private Filter incomingOrders;
        /// Orders that are pending for inventory refill
        private Filter pendingRefillOrders;
        private Event<InventoryItemRefillEvent> refillEvent;

        private DisposableTracker disposable;

        public override void OnAwake() {
            disposable = new DisposableTracker();
            inventoryFilter = World.Filter.With<Inventory>().Build();

            incomingOrders = World.Filter
                .With<ActiveState>().With<InventoryProductsOrder>()
                .Without<OrderApprovedState>().Without<InventoryRefillPending>()
                .Build();
            pendingRefillOrders = World.Filter
                .With<ActiveState>().With<InventoryProductsOrder>().With<InventoryRefillPending>()
                .Without<OrderApprovedState>()
                .Build();

            refillEvent = World.GetEvent<InventoryItemRefillEvent>();
            refillEvent.Subscribe(OnInventoryRefilled).AddTo(disposable);
        }

        public override void Dispose() {
            disposable.Dispose();
            
            // todo: Не понятно, как так происходит, что inventory не зануляется без перезагрузки домена!??
            inventory = null; 
        }

        public override void OnUpdate(float deltaTime) {
            inventory ??= inventoryFilter.FirstOrDefault().GetComponent<Inventory>().service;

            if (incomingOrders.IsEmpty()) return;
            // Debug.Log($"InventoryOrderSystem: new incoming orders is available");
            CheckOrders(incomingOrders, false);
        }

        /// Check orders when inventory has been refilled
        private void OnInventoryRefilled(FastList<InventoryItemRefillEvent> _) { // no need to foreach events list
            if (pendingRefillOrders.IsEmpty()) return;
            // Debug.Log($"InventoryOrderSystem: Check pending orders when inventory is refilled");
            CheckOrders(pendingRefillOrders, true);
        }

        private void CheckOrders(Filter filter, bool isPendingRefillOrders) {
            foreach (var orderEntity in filter) {
                var order = orderEntity.GetComponent<InventoryProductsOrder>();
                bool isMissingProducts = !order.IsEmpty && !inventory.Has(order.products);
                if (isMissingProducts) {
                    if (!isPendingRefillOrders) orderEntity.AddComponent<InventoryRefillPending>();
                    continue;
                }

                // Process order
                if (isPendingRefillOrders) orderEntity.RemoveComponent<InventoryRefillPending>();
                TakeOrder(orderEntity, order);
            }
        }

        private void TakeOrder(Entity orderEntity, InventoryProductsOrder order) {
            if (!order.IsEmpty) ProcessOrder(orderEntity, order);
            ApproveOrder(orderEntity);
        }

        /// Process inventory order
        private void ProcessOrder(Entity orderEntity, InventoryProductsOrder order) {
            if (order.IsEmpty) return;
            foreach ((var product, int count) in order.products) {
                // Check for inventory has a target product count
                if (count <= 0) {
                    Debug.LogError($"[Inventory] Order: count is enough! ({product.name}: {count})");
                    continue; // Issue log error, but do not break the process 
                }

                var itemEntity = inventory.Get(product);
                if (!product.stackable) {
                    World.RemoveEntity(itemEntity); // Remove orders entities from inventory
                } else {
                    ref var itemCount = ref itemEntity.GetComponent<Count>();
                    itemCount.ChangeSmart(-count, itemEntity); // subtract order count
                }
            }
        }

        private void ApproveOrder(Entity orderEntity) {
            orderEntity.AddComponent<OrderApprovedState>();
        }

    }
}