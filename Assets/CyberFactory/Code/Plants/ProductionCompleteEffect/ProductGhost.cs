using System;
using Scellecs.Morpeh;
using UnityEngine;

namespace CyberFactory.Plants.ProductionCompleteEffect {
    [Serializable]
    public struct ProductGhost : IComponent {

        public Transform transform;
        public Sprite sprite;
        public ProductGhostConfig config;

    }
}