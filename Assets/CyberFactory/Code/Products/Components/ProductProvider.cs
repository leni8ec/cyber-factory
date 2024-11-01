using Scellecs.Morpeh.Providers;
using TriInspector;
using UnityEngine;

namespace CyberFactory.Products.Components {

    [AddComponentMenu("Cyber Factory/Products/" + nameof(Product))] [HideMonoScript]
    public class ProductProvider : MonoProvider<Product> { }

}