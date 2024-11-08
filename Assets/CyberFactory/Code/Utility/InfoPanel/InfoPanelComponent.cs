using System;
using System.Collections.Generic;
using CyberFactory.Products.Models;
using Scellecs.Morpeh;
using TMPro;
using TriInspector;
using UnityEngine;
using UnityEngine.Serialization;

namespace CyberFactory.Utility.InfoPanel {
    [Serializable]
    public struct InfoPanelComponent : IComponent {

        public TextMeshProUGUI text;

        [PropertySpace]
        [Tooltip("Disable it for show all inventory items")]
        public bool useProductFilter;
        
        [FormerlySerializedAs("products")]
        [ShowIf(nameof(useProductFilter))]
        [Tooltip("Products to show")]
        public List<ProductModel> productFilter;

    }
}