using System;
using System.Collections;
using System.Collections.Generic;
using System.Text;
using CyberFactory.Basics.Objects;
using CyberFactory.Products.Models;
using TriInspector;
using UnityEngine;

namespace CyberFactory.Products.Objects {
    /// <summary>
    /// Set of { Product, count }
    /// </summary>
    [Serializable]
    public class ProductsSet : IEnumerable<PairValue<ProductModel, int>> {

        /// <summary>
        /// { Product, count }
        /// </summary>
        [SerializeField] [InlineProperty]
        private List<PairValue<ProductModel, int>> products;

        public int Count => products?.Count ?? 0;
        public bool IsEmpty => products == null || products.Count == 0;

        public IEnumerator<PairValue<ProductModel, int>> GetEnumerator() {
            foreach (PairValue<ProductModel, int> pairValue in products) yield return pairValue;
        }

        IEnumerator IEnumerable.GetEnumerator() {
            return GetEnumerator();
        }

        // note: used for tests
        public ProductsSet(List<PairValue<ProductModel, int>> products) {
            this.products = products;
        }

        public override string ToString() {
            var sb = new StringBuilder();
            sb.Append("{");
            foreach ((var product, int count) in products) {
                sb.Append(" ").Append(product.name).Append(": ").Append(count).Append(" ");
            }
            sb.Append("}");
            return sb.ToString();
        }

    }

}