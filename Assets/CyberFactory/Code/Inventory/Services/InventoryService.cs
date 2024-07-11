using System.Collections.Generic;
using CyberFactory.Products;
using Scellecs.Morpeh;

namespace CyberFactory.Inventory.Services {

    public class InventoryService {
        private readonly Dictionary<ProductVariant, Entity> items = new();
        public IReadOnlyDictionary<ProductVariant, Entity> Items => items;

        public bool Has(Product product) {
            return items.ContainsKey(product.product);
        }

        public Entity Get(Product product) {
            return items[product.product];
        }

        public void Add(Product product, Entity entity) {
            items.Add(product.product, entity);
        }

        public bool Remove(Product product) {
            return items.Remove(product.product);
        }

    }

}