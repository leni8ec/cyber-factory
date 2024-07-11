using System;
using CyberFactory.Products;
using Scellecs.Morpeh;
using Scellecs.Morpeh.Providers;

namespace CyberFactory.Inventory {

    public class InventoryProvider : MonoProvider<Inventory> { }

    [Serializable]
    public struct InventoryEntry {
        public ProductVariant product;
        public Entity entity;
    }

}