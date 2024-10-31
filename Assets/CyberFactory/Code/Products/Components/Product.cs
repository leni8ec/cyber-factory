using System;
using CyberFactory.Products.Configs;
using Scellecs.Morpeh;
using UnityEngine.Serialization;

namespace CyberFactory.Products.Components {

    [Serializable]
    public struct Product : IComponent {

        [FormerlySerializedAs("config")] public ProductConfig model;

        // public int count; moved to own component 'Count'

    }

}