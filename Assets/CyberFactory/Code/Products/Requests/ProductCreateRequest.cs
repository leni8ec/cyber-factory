using CyberFactory.Products.Models;
using Scellecs.Morpeh;

namespace CyberFactory.Products.Requests {
    public readonly struct ProductCreateRequest : IRequestData {

        public readonly Entity plantEntity;
        public readonly ProductModel product;
        public readonly int count;

        public ProductCreateRequest(Entity plantEntity, ProductModel product, int count) {
            this.plantEntity = plantEntity;
            this.product = product;
            this.count = count;
        }

    }
}