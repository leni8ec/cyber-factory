using System;
using CyberFactory.Products.Configs;
using Scellecs.Morpeh;

namespace CyberFactory.Products.Components {

    [Serializable]
    public struct Product : IComponent {

        public ProductVariant product;

        // public int count; moved to own component 'Count'

    }

}