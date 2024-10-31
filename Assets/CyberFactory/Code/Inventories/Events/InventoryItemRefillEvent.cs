using CyberFactory.Products.Configs;
using Scellecs.Morpeh;

namespace CyberFactory.Inventories.Events {
    /// <summary>
    /// Inventory replenishment
    /// <para/>
    /// An event that occurs when added new inventory item or increased count of exiting items (used for fabrics) 
    /// </summary>
    public struct InventoryItemRefillEvent : IEventData {

        public ProductConfig product;
        public int count;

    }
}