using CyberFactory.Products.Models;
using Scellecs.Morpeh;

namespace CyberFactory.Products.Events {
    public struct ProductCreatedEvent : IEventData {

        public readonly Entity plantEntity;
        public readonly Entity productEntity;
        public readonly ProductModel product;
        public readonly int count;

        public ProductCreatedEvent(Entity plantEntity, Entity productEntity, ProductModel product, int count) {
            this.plantEntity = plantEntity;
            this.productEntity = productEntity;
            this.product = product;
            this.count = count;
        }

    }
}