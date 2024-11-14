using System.Collections.Generic;
using CyberFactory.Basics.Constants.Editor;
using CyberFactory.Common.Models;
using CyberFactory.Products.Models;
using TriInspector;
using UnityEngine;

namespace CyberFactory.Plants.Core.Models {
    [CreateAssetMenu(menuName = AssetMenu.Plants.MODEL, fileName = "Plant", order = AssetMenu.Plants.ORDER + 10)] [HideMonoScript]
    public class PlantModel : ItemModelBase {

        [PropertySpace]
        public ProductModel product;

        [PropertySpace] [ListDrawerSettings(ShowElementLabels = true)]
        [InfoBox("Production rate (production cycles per second) \n\nDepends on the current station level")]
        // note: initial value - used for testing purposes and editor initial values
        public List<float> productionRateLevels = new() { 1, 2, 3 };

    }
}