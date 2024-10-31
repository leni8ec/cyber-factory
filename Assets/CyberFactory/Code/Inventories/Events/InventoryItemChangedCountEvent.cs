using CyberFactory.Products.Configs;
using Scellecs.Morpeh;

namespace CyberFactory.Inventories.Events {

    /// <summary>
    /// Called only when count is changed!
    /// <para/> Not for inventory item created or removed
    /// </summary>
    public struct InventoryItemChangedCountEvent : IEventData {

        public ProductConfig product;

        public int oldCount;
        public int newCount;


        public int Delta => newCount - oldCount;
        public bool IsIncrease => newCount > oldCount;

    }

}