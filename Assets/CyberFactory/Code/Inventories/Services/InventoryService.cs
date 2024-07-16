using System.Collections.Generic;
using CyberFactory.Common.Components;
using CyberFactory.Products.Components;
using CyberFactory.Products.Models;
using CyberFactory.Products.Objects;
using Scellecs.Morpeh;
using UnityEngine;

namespace CyberFactory.Inventories.Services {

    // todo: there is no implemented solution for storing not stackable items
    public class InventoryService {
        private readonly Dictionary<ProductModel, Entity> items = new();
        public IReadOnlyDictionary<ProductModel, Entity> Items => items;

        public int ItemsCount => items.Count;


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
            foreach ((var product, int count) in productsSet) {
                // Check for product count is valid (must be first for check validity of the request)
                bool productCountIsValid = product.stackable && count > 0;
                if (!productCountIsValid) {
                    Debug.LogError("[Inventory] request product count must be > 0");
                    return false;
                }

                if (!items.TryGetValue(product, out var itemEntity)) return false; //       if product exists
                var itemCount = itemEntity.GetComponent<Count>(out bool itemCountExists);
                if (product.stackable) { //                                                 if stackable 
                    if (!itemCountExists) { //                                                  - if inventory has not 'count' component
                        Debug.LogError("[Inventory] item - component 'count' is missing");
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
            return entity;
        }


        public int Count(ProductModel product) {
            return Get(product).GetComponent<Count>().value;
        }


        #region Sync

        public bool Add(Product product, Entity entity) {
            return items.TryAdd(product.model, entity);
        }

        public bool Remove(Product product) {
            return items.Remove(product.model);
        }

        #endregion


    }

}