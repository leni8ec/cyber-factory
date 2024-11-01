using CyberFactory.Basics.Constants.Editor;
using CyberFactory.Common.Components;
using CyberFactory.Inventories.Components;
using CyberFactory.Inventories.Events;
using CyberFactory.Inventories.Requests;
using CyberFactory.Inventories.Services;
using Scellecs.Morpeh;
using Scellecs.Morpeh.Collections;
using Scellecs.Morpeh.Systems;
using UnityEngine;

namespace CyberFactory.Inventories.Systems {

    /// <summary>
    /// Process requests to release items to other entities (ex: for fabrics)
    /// note: Order - [Second] (or [First] if without 'Release' system)
    /// </summary>
    [CreateAssetMenu(menuName = AssetMenu.Systems.INVENTORY + "Request", fileName = nameof(InventoryRequestSystem), order = AssetMenu.Systems.INVENTORY_ORDER)]
    public sealed class InventoryRequestSystem : UpdateSystem {

        private Filter inventories;
        private Filter newRequests;
        private Filter waitForRefillRequests;
        private Event<InventoryItemRefillEvent> refillEvent;

        public override void OnAwake() {
            inventories = World.Filter.With<Inventory>().Build();
            newRequests = World.Filter
                .With<ActiveState>().With<InventoryProductsRequest>()
                .Without<RequestApprovedState>().Without<InventoryWaitForRefill>()
                .Build();
            waitForRefillRequests = World.Filter
                .With<ActiveState>().With<InventoryProductsRequest>().With<InventoryWaitForRefill>()
                .Without<RequestApprovedState>()
                .Build();

            refillEvent = World.GetEvent<InventoryItemRefillEvent>();
            refillEvent.Subscribe(OnInventoryRefilled);
        }

        public override void OnUpdate(float deltaTime) {
            if (newRequests.IsEmpty()) return;
            // Debug.Log($"InventoryRequestSystem: Check new requests");
            var service = inventories.FirstOrDefault().GetComponent<Inventory>().service;
            CheckRequests(newRequests, false, service);
        }

        // Check requests when inventory has been refilled
        private void OnInventoryRefilled(FastList<InventoryItemRefillEvent> _) { // no need to foreach events list
            if (waitForRefillRequests.IsEmpty()) return;
            // Debug.Log($"InventoryRequestSystem: OnInventoryRefilled");
            var service = inventories.FirstOrDefault().GetComponent<Inventory>().service;
            CheckRequests(waitForRefillRequests, true, service);
        }

        private void CheckRequests(Filter filter, bool isWaitingRequests, InventoryService service) {
            foreach (var requestEntity in filter) {
                var request = requestEntity.GetComponent<InventoryProductsRequest>();
                bool isItemsAvailable = request.IsEmpty || service.Has(request.products);
                if (!isItemsAvailable) {
                    if (!isWaitingRequests) requestEntity.AddComponent<InventoryWaitForRefill>();
                    continue;
                }

                // Approve request
                if (isWaitingRequests) requestEntity.RemoveComponent<InventoryWaitForRefill>();
                ApproveRequest(service, requestEntity, request);
            }
        }

        private void ApproveRequest(InventoryService service, Entity requestEntity, InventoryProductsRequest request) {
            // Approve request
            requestEntity.AddComponent<RequestApprovedState>();

            // Remove requests entities from inventory
            if (request.IsEmpty) return;
            foreach ((var product, int count) in request.products) {
                if (count <= 0) {
                    Debug.LogWarning($"[Inventory] Request: count is {count} ({product.name})");
                    continue; // skip if request item count is empty
                }

                var inventoryItemEntity = service.Get(product);
                if (!product.stackable) {
                    World.RemoveEntity(inventoryItemEntity);
                } else {
                    ref var inventoryItemCount = ref inventoryItemEntity.GetComponent<Count>();
                    inventoryItemCount.Change(-count, out var changedCount); // subtract request count
                    inventoryItemEntity.AddComponent<ChangedCount>() = changedCount;
                }
            }
        }

    }
}