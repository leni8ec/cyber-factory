using System.Collections.Generic;
using CyberFactory.Basics.Constants.Editor;
using CyberFactory.Products.Models;
using TriInspector;
using UnityEngine;

namespace CyberFactory.Plants.Models {
    [CreateAssetMenu(menuName = AssetMenu.Models.PLANT, fileName = "Plant", order = AssetMenu.Models.PLANT_ORDER)] [HideMonoScript]
    public class PlantModel : ScriptableObject {

        [PropertySpace]
        public ProductModel product;

        [PropertySpace] [ListDrawerSettings(ShowElementLabels = true)]
        [InfoBox("Production rate (in pcs per second) \n\nDepends on the current station level")]
        // note: initial value - used for testing purposes and editor initial values
        public List<float> productionRateLevels = new() { 1, 2, 3 };

    }
}