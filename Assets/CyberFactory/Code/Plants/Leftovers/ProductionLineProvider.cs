using System;
using CyberFactory.Products.Deprecated;
using Scellecs.Morpeh;
using Scellecs.Morpeh.Providers;
using UnityEngine;

namespace CyberFactory.Plants.Leftovers {

    public class ProductionLineProvider : MonoProvider<ProductionLine> { }

    [Serializable]
    public struct ProductionLine : IComponent {
        [SerializeField]
        public IStuffType type;
    }

}