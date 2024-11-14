using CyberFactory.Basics.Constants.Editor;
using Scellecs.Morpeh.Providers;
using TriInspector;
using UnityEngine;

namespace CyberFactory.Plants.Core.Components {

    [AddComponentMenu(AssetMenu.Plants.PROVIDER + nameof(Plant))] [HideMonoScript]
    public class PlantProvider : MonoProvider<Plant> { }

}