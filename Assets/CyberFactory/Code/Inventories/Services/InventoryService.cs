using System;
using System.Collections.Generic;
using CyberFactory.Common.Components;
using CyberFactory.Products.Components;
using CyberFactory.Products.Models;
using CyberFactory.Products.Objects;
using Scellecs.Morpeh;
using UnityEngine;

namespace CyberFactory.Inventories.Services {

    // todo: there is no implemented solution for storing not stackable items
    public class InventoryService : IDisposable {
        private Dictionary<ProductModel, Entity> items = new();
        public IReadOnlyDictionary<ProductModel, Entity> Items => items;

        public int ItemsCount => items.Count;

        public void Dispose() {
            items.Clear();
            items = null;
        }

        public bool Has(Product product) {
            return Has(product.model);
        }

        public bool Has(ProductModel product) {
            return items.ContainsKey(product);
        }

        public bool Has(Product product, int count) {
            return Has(product.model, count);
        }

        public bool Has(ProductsSet productsSet) {
            Debug.Log(this.GetHashCode());
            if (productsSet.IsEmpty) {
                Debug.LogWarning("[Inventory] Checked 'ProductSet' is empty!");
                return true;
            }
            foreach ((var product, int count) in productsSet) {
                switch (count) {
                    case < 0: // Check for product count request is valid (must be first for check validity of the request)
                        Debug.LogWarning($"[Inventory] Request: product count must be >= 0 ({product.name}: {count})");
                        return false; // request requirement is not valid
                    case 0: // Check for product count request is empty - skip check for this
                        // Debug.LogWarning($"[Inventory] Request: product count is 0 ({product.name})");
                        continue; // request item requirement is empty
                }


                if (!items.TryGetValue(product, out var itemEntity)) // Check for product exists
                    return false;
                if (itemEntity.IsDisposed()) { // Check for valid entity
                    Debug.LogWarning("[Inventory] Entity of the inventory item - has been disposed before");
                    return false;
                }
                var itemCount = itemEntity.GetComponent<Count>(out bool itemCountExists);
                if (product.stackable) { //                                                 if stackable 
                    if (!itemCountExists) { //                                                  - if inventory has not 'count' component
                        Debug.LogWarning("[Inventory] item - component 'count' is missing");
                        return false;
                    }
                    if (itemCount.value < count) { //                                           - if inventory count is enough
                        return false;
                    }
                }
            }
            return true;
        }

        public bool Has(ProductModel product, int count) {
            if (!Has(product)) return false;

            int hasCount = Get(product).GetComponent<Count>().value;
            return hasCount >= count;
        }


        public Entity Get(Product product) {
            return Get(product.model);
        }

        public Entity Get(ProductModel product) {
            return items[product];
        }


        public Entity TryGet(Product product, out bool exists) {
            return TryGet(product.model, out exists);
        }

        public Entity TryGet(ProductModel product, out bool exists) {
            exists = items.TryGetValue(product, out var entity);
            if (exists && entity.IsNullOrDisposed()) {
                Debug.LogWarning("[Inventory] Entity of the inventory item - has been disposed before");
                exists = false;
            }
            return entity;
        }


        public int Count(ProductModel product) {
            // return Get(product).GetComponent<Count>().value; // old realization

            var productEntity = TryGet(product, out bool exists);
            if (!exists) return 0;
            if (!product.stackable) return 1;
            return productEntity.GetComponent<Count>().value;
        }


        #region Sync

        /// Try to synchronize inventory when adding a new entity
        public bool TrySyncOnAdd(Product product, Entity entity) {
            return items.TryAdd(product.model, entity);
        }

        /// Try to synchronize inventory when entity has been removed
        public bool TrySyncOnRemove(Product product) {
            return items.Remove(product.model);
        }

        #endregion


    }

}