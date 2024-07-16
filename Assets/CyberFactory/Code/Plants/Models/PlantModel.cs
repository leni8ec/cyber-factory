using System.Collections.Generic;
using CyberFactory.Products.Models;
using TriInspector;
using UnityEngine;

namespace CyberFactory.Plants.Models {

    [HideMonoScript]
    [CreateAssetMenu(fileName = "Plant", menuName = "Stuff/Plant", order = 0)]
    public class PlantModel : ScriptableObject {

        [PropertySpace]
        public ProductModel product;

        [PropertySpace] [ListDrawerSettings(ShowElementLabels = true)]
        [Tooltip("Production rate (in pcs per second) \n\nDepends on the current station level")]
        public List<float> productionRateLevels = new() { 1 };
    }

}