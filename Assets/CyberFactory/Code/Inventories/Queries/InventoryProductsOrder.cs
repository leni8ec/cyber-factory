using System;
using CyberFactory.Common.Queries;
using CyberFactory.Products.Objects;

namespace CyberFactory.Inventories.Queries {
    /// <summary>
    /// This request must be approved with `RequestApprovedState`
    /// </summary>
    [Serializable]
    public struct InventoryProductsOrder : IQueryComponent {

        public ProductsSet products;


        public bool IsEmpty => products is null || products.IsEmpty;

    }
}