using CyberFactory.Products;
using Scellecs.Morpeh;

namespace CyberFactory.Inventory {

    public struct InventoryItemChangedEvent : IEventData {

        public ProductVariant product;
        public long count;

    }

}