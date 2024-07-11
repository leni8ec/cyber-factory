using System.Collections.Generic;
using CyberFactory.Basics.Objects;
using TriInspector;
using UnityEngine;

namespace CyberFactory.Products {

    [HideMonoScript]
    [CreateAssetMenu(fileName = "Product", menuName = "Stuff/Product", order = 0)]
    public class ProductVariant : ScriptableObject {

        [PropertySpace]
        public int price;

        [PropertySpace]
        [Tooltip("Ingredients list \n\n{ stuff : count } \n\n (leave empty if not required) ")]
        public List<PairValue<ProductVariant, int>> recipe;

    }

}