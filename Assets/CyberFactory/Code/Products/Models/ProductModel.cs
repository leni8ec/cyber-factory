using CyberFactory.Basics.Constants.Editor;
using CyberFactory.Products.Objects;
using TriInspector;
using UnityEngine;
using UnityEngine.AddressableAssets;

namespace CyberFactory.Products.Models {
    [CreateAssetMenu(menuName = AssetMenu.Models.PRODUCT, fileName = "Plant", order = AssetMenu.Models.PRODUCT_ORDER)] [HideMonoScript]
    public class ProductModel : ScriptableObject {

        [PropertySpace]
        public AssetReferenceSprite icon;

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