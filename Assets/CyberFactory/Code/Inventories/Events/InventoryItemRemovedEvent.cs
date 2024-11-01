using CyberFactory.Products.Models;
using Scellecs.Morpeh;

namespace CyberFactory.Inventories.Events {
    /// <summary>
    /// An event that occurs when removed any inventory items 
    /// </summary>
    public struct InventoryItemRemovedEvent : IEventData {

        public ProductModel product;

    }
}