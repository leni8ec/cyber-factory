using System.Collections.Generic;
using CyberFactory.Products.Configs;
using TriInspector;
using UnityEngine;

namespace CyberFactory.Plants.Configs {
    [CreateAssetMenu(fileName = "Plant", menuName = "Stuff/Plant", order = 0)] [HideMonoScript]
    public class PlantConfig : ScriptableObject {

        [PropertySpace]
        public ProductConfig product;

        [PropertySpace] [ListDrawerSettings(ShowElementLabels = true)]
        [InfoBox("Production rate (in pcs per second) \n\nDepends on the current station level")]
        // note: initial value - used for testing purposes and editor initial values
        public List<float> productionRateLevels = new() { 1, 2, 3 };

    }
}