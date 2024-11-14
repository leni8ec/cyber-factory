using CyberFactory.Products.Models;
using Scellecs.Morpeh;

namespace CyberFactory.Products.Requests {
    public readonly struct CreateProductRequest : IRequestData {

        public readonly Entity plantEntity;
        public readonly ProductModel product;
        public readonly int count;

        public CreateProductRequest(Entity plantEntity, ProductModel product, int count) {
            this.plantEntity = plantEntity;
            this.product = product;
            this.count = count;
        }

    }
}