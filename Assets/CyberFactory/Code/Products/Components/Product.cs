using System;
using CyberFactory.Products.Models;
using Scellecs.Morpeh;

namespace CyberFactory.Products.Components {

    [Serializable]
    public struct Product : IComponent {

        public ProductModel model;

        // public int count; moved to own component 'Count'

    }

}