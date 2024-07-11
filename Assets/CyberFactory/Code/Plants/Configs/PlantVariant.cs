using System.Collections.Generic;
using CyberFactory.Products.Configs;
using TriInspector;
using UnityEngine;

namespace CyberFactory.Plants.Configs {

    [HideMonoScript]
    [CreateAssetMenu(fileName = "Plant", menuName = "Stuff/Plant", order = 0)]
    public class PlantVariant : ScriptableObject {

        [PropertySpace]
        public ProductVariant product;

        [PropertySpace] [ListDrawerSettings(ShowElementLabels = true)]
        [Tooltip("Production rate (in pcs per second) \n\nDepends on the current station level")]
        public List<float> productionRateLevels = new() { 1 };
    }

}