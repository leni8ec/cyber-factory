using CyberFactory.Products.Models;
using Scellecs.Morpeh;

namespace CyberFactory.Plants.Events {
    public readonly struct ProductionCompleteEvent : IEventData {

        public readonly Entity plantEntity;
        public readonly ProductModel product;
        public readonly int count;

        public ProductionCompleteEvent(Entity plantEntity, ProductModel product, int count = 1) {
            this.plantEntity = plantEntity;
            this.product = product;
            this.count = count;
        }

    }
}