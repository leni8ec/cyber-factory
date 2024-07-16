using CyberFactory.Products.Models;
using Scellecs.Morpeh;

namespace CyberFactory.Inventories.Events {

    /// <summary>
    /// An event that occurs when added new instance of inventory item 
    /// </summary>
    public struct InventoryItemCreatedEvent : IEventData {

        public ProductModel product;
        public int count;

    }
}