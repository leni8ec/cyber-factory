using System;
using CyberFactory.Products.Models;
using Scellecs.Morpeh;

namespace CyberFactory.Inventories.Basics {

    [Serializable]
    public struct InventoryEntry {
        public ProductModel product;
        public Entity entity;
    }

}