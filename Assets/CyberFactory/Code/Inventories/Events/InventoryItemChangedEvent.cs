using CyberFactory.Products.Configs;
using Scellecs.Morpeh;

namespace CyberFactory.Inventories.Events {

    public struct InventoryItemChangedEvent : IEventData {

        public ProductVariant product;
        public long count;

    }

}