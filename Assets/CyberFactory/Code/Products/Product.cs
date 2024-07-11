using System;
using Scellecs.Morpeh;

namespace CyberFactory.Products {

    [Serializable]
    public struct Product : IComponent {

        public ProductVariant product;

        // public int count; moved to own component 'Count'

    }

}