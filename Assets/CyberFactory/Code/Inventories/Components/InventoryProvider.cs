using System;
using CyberFactory.Products.Configs;
using Scellecs.Morpeh;
using Scellecs.Morpeh.Providers;

namespace CyberFactory.Inventories.Components {

    public class InventoryProvider : MonoProvider<Inventory> { }

    [Serializable]
    public struct InventoryEntry {
        public ProductConfig product;
        public Entity entity;
    }

}