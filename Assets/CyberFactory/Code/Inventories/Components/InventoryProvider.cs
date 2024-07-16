using System;
using CyberFactory.Products.Models;
using Scellecs.Morpeh;
using Scellecs.Morpeh.Providers;

namespace CyberFactory.Inventories.Components {

    public class InventoryProvider : MonoProvider<Inventory> { }

    [Serializable]
    public struct InventoryEntry {
        public ProductModel product;
        public Entity entity;
    }

}