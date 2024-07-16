using CyberFactory.Products.Models;
using Scellecs.Morpeh;

namespace CyberFactory.Inventories.Events {
    /// <summary>
    /// Inventory replenishment
    /// <para/>
    /// An event that occurs when added new inventory item or increased count of exiting items (used for fabrics) 
    /// </summary>
    public struct InventoryItemRefillEvent : IEventData {

        public ProductModel product;
        public int count;

    }
}