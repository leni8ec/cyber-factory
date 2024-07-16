using CyberFactory.Products.Objects;
using TriInspector;
using UnityEngine;

namespace CyberFactory.Products.Models {

    [HideMonoScript]
    [CreateAssetMenu(fileName = "Product", menuName = "Stuff/Product", order = 0)]
    public class ProductModel : ScriptableObject {

        [PropertySpace]
        public int price;

        [PropertySpace]
        [Tooltip("Product can be stacked in the inventory \n(enabled by default) \n\nNot stackable:\n  - each object is a separate element\n  - 'count' not applicable")]
        public bool stackable = true;

        [PropertySpace]
        [Tooltip("Ingredients list \n\n{ stuff : count } \n\n (leave empty if not required) ")]
        public ProductsSet recipe;


        public bool HasRecipe => recipe.Count == 0;

    }

}