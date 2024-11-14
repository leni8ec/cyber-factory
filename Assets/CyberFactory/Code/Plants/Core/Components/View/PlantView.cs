using System;
using CyberFactory.Plants.ProductionCompleteEffect;
using Scellecs.Morpeh;
using UnityEngine;
using UnityEngine.UI;

namespace CyberFactory.Plants.Core.Components.View {
    [Serializable]
    public struct PlantView : IComponent {

        public Transform transform;
        public Image progressBar;
        public ProductGhostConfig productGhostConfig;

    }
}