using System;
using CyberFactory.Products.Objects;
using Scellecs.Morpeh;

namespace CyberFactory.Inventories.Requests {
    /// <summary>
    /// This request must be approved with `RequestApprovedState`
    /// </summary>
    [Serializable]
    public struct InventoryProductsRequest : IComponent {

        public ProductsSet products;


        public bool IsEmpty => products is null || products.IsEmpty;

    }
}