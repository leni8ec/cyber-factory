using CyberFactory.Basics.Constants.Editor;
using CyberFactory.Common.Components;
using CyberFactory.Inventories.Queries;
using CyberFactory.Products.Components;
using CyberFactory.Products.Events;
using CyberFactory.Products.Requests;
using Scellecs.Morpeh;
using Scellecs.Morpeh.Collections;
using Scellecs.Morpeh.Systems;
using UnityEngine;

namespace CyberFactory.Products.Systems {

    [CreateAssetMenu(menuName = AssetMenu.Products.SYSTEM + nameof(ProductCreationSystem), order = AssetMenu.Products.ORDER)]
    public sealed class ProductCreationSystem : UpdateSystem {
        private Request<ProductCreateRequest> createRequest;

        public override void OnAwake() {
            createRequest = World.GetRequest<ProductCreateRequest>();
        }

        public override void OnUpdate(float deltaTime) {
            foreach (var createProductRequest in createRequest.Consume()) {
                var productEntity = World.CreateEntity();
                productEntity.AddComponent<Product>().model = createProductRequest.product;
                productEntity.AddComponent<Count>().value = createProductRequest.count;

                productEntity.AddComponent<InventoryItemPullCall>();

                // Publish event
                World.GetEvent<ProductCreatedEvent>().NextFrame(new ProductCreatedEvent(
                    createProductRequest.plantEntity,
                    productEntity,
                    createProductRequest.product,
                    createProductRequest.count));
            }
        }

    }
}