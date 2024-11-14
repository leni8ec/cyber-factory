using CyberFactory.Basics.Constants.Editor;
using TriInspector;
using UnityEngine;

namespace CyberFactory.Plants.ProductionCompleteEffect {
    [CreateAssetMenu(fileName = "ProductGhost", menuName = AssetMenu.Plants.CONFIG + "Product Ghost (on production complete)", order = AssetMenu.Plants.ORDER)]
    // [CreateAssetMenu(fileName = "ProductGhost", menuName = "Config/ProductGhost", order = 0)]
    public class ProductGhostConfig : ScriptableObject {

        [InfoBox("The effect of producing a resource when the icon of the produced resource rises up")]
        [Tooltip("Ghost lifetime in seconds")]
        public float duration;

        [Tooltip("StartOffset of the ghost object")]
        public Vector3 startOffset;
        [Tooltip("Moving distance of the ghost object")]
        public Vector3 moveDistance;

    }
}